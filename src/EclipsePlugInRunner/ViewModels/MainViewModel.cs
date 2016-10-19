using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EclipsePlugInRunner.Data;
using EclipsePlugInRunner.Helpers;
using EclipsePlugInRunner.Scripting;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        private const int MaximumNumberOfRecentPatientContexts = 5;

        private readonly ScriptProxy _scriptProxy;
        private readonly SettingsRepository _settingsRepo;

        private Application _app;
        private Patient _patient;

        public MainViewModel(object script)
        {
            _scriptProxy = new ScriptProxy(script);
            _settingsRepo = new SettingsRepository();

            OpenPatientContextCommand = new RelayCommand(OpenPatientContext);
            RemovePatientContextCommand = new RelayCommand(RemovePatientContext);
            RemoveAllPatientContextsCommand = new RelayCommand(RemoveAllPatientContexts);
            RunWithPatientContextCommand = new RelayCommand(RunWithPatientContext);

            OpenPatientCommand = new RelayCommand(OpenPatient);
            RunScriptCommand = new RelayCommand(RunScript);
            ExitCommand = new RelayCommand(Exit);

            LoadSettings();
        }

        public event EventHandler ExitRequested;
        public event EventHandler<string> UserMessaged;

        public ICommand OpenPatientContextCommand { get; private set; }
        public ICommand RemovePatientContextCommand { get; private set; }
        public ICommand RemoveAllPatientContextsCommand { get; private set; }
        public ICommand RunWithPatientContextCommand { get; private set; }

        public ICommand OpenPatientCommand { get; private set; }
        public ICommand RunScriptCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }

        private string _patientId;
        public string PatientId
        {
            get { return _patientId; }
            set { Set(ref _patientId, value); }
        }

        private IEnumerable<PlanningItemViewModel> _planningItems;
        public IEnumerable<PlanningItemViewModel> PlanningItems
        {
            get { return _planningItems; }
            private set { Set(ref _planningItems, value); }
        }

        private ObservableCollection<PlanningItemViewModel> _planSetupsInScope;
        public ObservableCollection<PlanningItemViewModel> PlanSetupsInScope
        {
            get { return _planSetupsInScope; }
            private set { Set(ref _planSetupsInScope, value); }
        }

        private PlanningItemViewModel _selectedPlanSetup;
        public PlanningItemViewModel SelectedPlanSetup
        {
            get { return _selectedPlanSetup; }
            set { Set(ref _selectedPlanSetup, value); }
        }

        private bool _shouldExit;
        public bool ShouldExit
        {
            get { return _shouldExit; }
            set { Set(ref _shouldExit, value); }
        }

        public ObservableCollection<PatientContext> RecentPatientContexts { get; private set; }

        private PatientContext _selectedPatientContext;
        public PatientContext SelectedPatientContext
        {
            get { return _selectedPatientContext; }
            set { Set(ref _selectedPatientContext, value); }
        }

        public void StartEclipse()
        {
            _app = Application.CreateApplication(null, null);
        }

        public void StopEclipse()
        {
            _app.Dispose();
        }

        protected virtual void OnExitRequested()
        {
            if (ExitRequested != null)
            {
                ExitRequested(this, EventArgs.Empty);
            }
        }

        protected virtual void OnUserMessaged(string message)
        {
            if (UserMessaged != null)
            {
                UserMessaged(this, message);
            }
        }

        private void OpenPatientContext()
        {
            PatientId = SelectedPatientContext.PatientId;

            OpenPatient();

            foreach (var planningItem in SelectedPatientContext.PlanningItemsInScope)
            {
                var pi = PlanningItems
                    .First(p => p.Id == planningItem.Id && p.CourseId == planningItem.CourseId);
                pi.IsChecked = true;  // Will add it to PlanSetupsInScope
            }

            SelectedPlanSetup = PlanSetupsInScope
                .Single(p => p.Id == SelectedPatientContext.ActivePlanSetup.Id
                             && p.CourseId == SelectedPatientContext.ActivePlanSetup.CourseId);
        }

        private void RemovePatientContext()
        {
            RecentPatientContexts.Remove(SelectedPatientContext);
            SelectedPatientContext = null;
        }

        private void RemoveAllPatientContexts()
        {
            RecentPatientContexts.Clear();
            SelectedPatientContext = null;
        }

        private void RunWithPatientContext()
        {
            OpenPatientContext();
            RunScript();
        }

        private void OpenPatient()
        {
            _app.ClosePatient();   // Close previous patient, if any
            _patient = _app.OpenPatientById(PatientId);

            if (_patient == null)
            {
                OnUserMessaged("The patient \"" + PatientId + "\" was not found.");
                return;
            }

            PlanningItems = CreatePlanningItems();

            PlanSetupsInScope = new ObservableCollection<PlanningItemViewModel>();
            SelectedPlanSetup = null;

            UpdatePlanSetupsInScopeWhenPlanSetupVmIsCheckedChanged();
        }

        private IEnumerable<PlanningItemViewModel> CreatePlanningItems()
        {
            // Convert to a List so that the PlanningItems are created immediately;
            // otherwise, there will be problems with listening to PropertyChanged
            return _patient.GetPlanningItems()
                .Select(p => new PlanningItemViewModel(p.Item1, p.Item2)).ToList();
        }

        private void UpdatePlanSetupsInScopeWhenPlanSetupVmIsCheckedChanged()
        {
            // Each planSetupVm is a PlanningItemViewModel that contains a PlanSetup
            foreach (var planSetupVm in PlanningItems.Where(p => p.PlanningItem is PlanSetup))
            {
                planSetupVm.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "IsChecked")
                    {
                        UpdatePlanSetupsInScope((PlanningItemViewModel)o);
                    }
                };
            }
        }

        private void UpdatePlanSetupsInScope(PlanningItemViewModel planSetupVm)
        {
            if (planSetupVm.IsChecked)
            {
                PlanSetupsInScope.Add(planSetupVm);
            }
            else
            {
                PlanSetupsInScope.Remove(planSetupVm);
            }
        }

        private void RunScript()
        {
            UpdateRecentPatientContexts();

            _scriptProxy.RunWithNewWindow(CreatePlugInScriptContext());

            if (ShouldExit)
            {
                Exit();
            }
        }

        private PlugInScriptContext CreatePlugInScriptContext()
        {
            return new PlugInScriptContext(
                _app.CurrentUser,
                _patient,
                null,    // Image
                null,    // StructureSet
                GetActivePlanSetup(),
                GetPlanSetupsInScope(),
                GetPlanSumsInScope());
        }

        private PlanSetup GetActivePlanSetup()
        {
            return SelectedPlanSetup != null
                ? (PlanSetup)SelectedPlanSetup.PlanningItem
                : null;
        }

        private IEnumerable<PlanSetup> GetPlanSetupsInScope()
        {
            return PlanSetupsInScope.Select(p => p.PlanningItem).Cast<PlanSetup>();
        }

        private IEnumerable<PlanSum> GetPlanSumsInScope()
        {
            return PlanningItems
                .Where(p => p.PlanningItem is PlanSum && p.IsChecked)
                .Select(p => p.PlanningItem)
                .Cast<PlanSum>();
        }

        private void Exit()
        {
            WriteSettings();
            OnExitRequested();
        }

        private void UpdateRecentPatientContexts()
        {
            var patientContext = new PatientContext();

            patientContext.PatientId = _patientId;
            patientContext.ActivePlanSetup = MapPlanningItemViewModelToData(SelectedPlanSetup);

            foreach (var planningItemsInScope in PlanningItems.Where(p => p.IsChecked))
            {
                patientContext.PlanningItemsInScope
                    .Add(MapPlanningItemViewModelToData(planningItemsInScope));
            }

            if (RecentPatientContexts.Contains(patientContext))
            {
                var index = RecentPatientContexts.IndexOf(patientContext);
                RecentPatientContexts.Move(index, 0);
            }
            else
            {
                RecentPatientContexts.Insert(0, patientContext);

                if (RecentPatientContexts.Count > MaximumNumberOfRecentPatientContexts)
                {
                    RecentPatientContexts.Remove(RecentPatientContexts.Last());
                }
            }
        }

        private Data.PlanningItem MapPlanningItemViewModelToData(
            PlanningItemViewModel planningItemVm)
        {
            return new Data.PlanningItem
            {
                Id = planningItemVm.Id,
                CourseId = planningItemVm.CourseId
            };
        }

        private void LoadSettings()
        {
            var settings = _settingsRepo.ReadSettings();

            if (settings != null)
            {
                ShouldExit = settings.ShouldExitAfterScriptEnds;
                RecentPatientContexts =
                    new ObservableCollection<PatientContext>(settings.RecentPatientContexts);
            }
        }

        private void WriteSettings()
        {
            var settings = new Settings();

            settings.ShouldExitAfterScriptEnds = ShouldExit;
            settings.RecentPatientContexts = RecentPatientContexts.ToList();

            _settingsRepo.WriteSettings(settings);
        }
    }
}