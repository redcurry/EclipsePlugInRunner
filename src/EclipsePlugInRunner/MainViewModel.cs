using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EclipsePlugInRunner.Data;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using VMS.TPS.Common.Model.API;

namespace EclipsePlugInRunner
{
    internal class MainViewModel : ViewModelBase
    {
        private readonly ScriptProxy _scriptProxy;
        private readonly SettingsRepository _settingsRepo;

        private Application _app;
        private Patient _patient;

        public MainViewModel(object script)
        {
            _scriptProxy = new ScriptProxy(script);
            _settingsRepo = new SettingsRepository();

            OpenPatientCommand = new RelayCommand(OpenPatient);
            RunScriptCommand = new RelayCommand(RunScript);
            ExitCommand = new RelayCommand(Exit);

            LoadSettings();
        }

        public event EventHandler ExitRequested;

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

        private void OpenPatient()
        {
            _app.ClosePatient();   // Close previous patient, if any
            _patient = _app.OpenPatientById(PatientId);

            PlanningItems = CreatePlanningItems();

            PlanSetupsInScope = new ObservableCollection<PlanningItemViewModel>();
            SelectedPlanSetup = null;

            UpdatePlanSetupsInScopeWhenPlanSetupVmIsCheckedChanged();
        }

        private IEnumerable<PlanningItemViewModel> CreatePlanningItems()
        {
            // Convert to a List so that the PlanningItems are created immediately;
            // otherwise, there will be problems with listening to PropertyChanged
            return _patient.GetPlanningItems().Select(p => new PlanningItemViewModel(p)).ToList();
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
            OnExitRequested();
        }

        private void LoadSettings()
        {
            var settings = _settingsRepo.ReadSettings();

            ShouldExit = settings.ShouldExitAfterScriptEnds;
            RecentPatientContexts =
                new ObservableCollection<PatientContext>(settings.RecentPatientContexts);
        }
    }
}