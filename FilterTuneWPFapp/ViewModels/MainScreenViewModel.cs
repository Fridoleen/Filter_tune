using FilterTuneLibrary;
using FilterTuneWPF_dll;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace FilterTuneWPF
{
    class MainScreenViewModel : Notifier
    {
        #region Values
        private string templateFileName = "TemplatesCollection.xml";
        private TemplateManager TemplatesSource;
        private TemplateViewModel chosenTemplate;
        public ObservableCollection<TemplateViewModel> Templates { get; set; }
        public SourceFilters ViewFilters { get; set; }
        public TemplateViewModel ChosenTemplate
        {
            get => chosenTemplate; set
            {
                chosenTemplate = value;
                NotifyPropertyChanged("ChosenTemplate");
            }
        }
        private Settings FTSettings; 
        public string FilterTargetName {
            get 
            {
                return (FTSettings.FilterFileTarget);
            }
            set 
            {
                FTSettings.FilterFileTarget = value;
                NotifyPropertyChanged("FilterTargetName");
            }
        }
        #endregion

        #region Commands
        public ICommand SaveFilterCommand { get; set; }
        public ICommand SaveTemplateCommand { get; set; }
        public ICommand RemoveTemplateCommand { get; set; }
        public ICommand FilterPathSourceCommand { get; set; }
        public ICommand FilterPathTargetCommand { get; set; }
        #endregion

        #region Saving and loading
        private ObservableCollection<TemplateViewModel> LoadTemplates(int numberOfTemplates=5)
        {
            var templates = new ObservableCollection<TemplateViewModel>();
            //creating mock templates if there is something wrong with the file
            if (templateFileName == "")
            {
                for (int i = 0; i < numberOfTemplates; i++)
                {
                    templates.Add(new TemplateViewModel($"This is the {i}th selector", $"This is the {i}th parameter", $"Template {i}"));
                }
            }
            else
            {
                //var templatesSource = new TemplateManager(Directory.GetCurrentDirectory()+"\\"+ templateFileName);
                var templatesFromFile = TemplatesSource.GetTemplates();
                foreach (FilterTemplate template in templatesFromFile.Templates)
                {
                    templates.Add(new TemplateViewModel(template.SelectorsText, template.ParametersText, template.Name));
                }
            }
            return (templates);
        }
        private void SaveTemplatesToFile()
        {
            // transfrom Templates to TemplateList
            List<FilterTemplate> templatesList = new List<FilterTemplate>();
            foreach (var template in Templates) //selectors parameters templatename
            {
                var libTemplate = new FilterTemplate(template.TemplateName, template.Selectors, template.Parameters);
                templatesList.Add(libTemplate);
            }
            var templatesLibList = new TemplateList(templatesList);
            TemplatesSource.SaveTemplates(templatesLibList);
        }

        private void SaveFilter() //relies on the library
        {
            SaveTemplatesToFile();
        }
        private ObservableCollection<TemplateViewModel> MockSavedTemplates { get; set; }
        public void SaveTemplate() //update Templates with NewTemplateName value of chosenTemplate
        {
            Templates.Add(new TemplateViewModel(chosenTemplate.Selectors, chosenTemplate.Parameters, chosenTemplate.NewTemplateName));//TODO check if it's an old template; provide the choice to overwrite or add new
            SaveTemplatesToFile();
        }
        public void RemoveTemplate() 
        {
            var result = MessageBox.Show($"Are you sure you want to delete {ChosenTemplate.TemplateName}?", "Deletion confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes) 
            {
                Templates.Remove(ChosenTemplate);
            }           
        }
        private string OpenFilterDialogue()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Filter";
            dlg.DefaultExt = ".filter";
            dlg.Filter = "PoE filter (.filter)|*.filter";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                return (dlg.FileName);
            }
            else
            {
                return "";
            }
        }

        private void RefreshFilterList()
        {
            //uses FTSettings to refresh ViewFilters
            if (Directory.Exists(FTSettings.FilterPathSource))
            {
                string[] getFilters = Directory.GetFiles(FTSettings.FilterPathSource, "*.filter");
                for(int i=0; i<getFilters.Length; i++)
                {
                    getFilters[i] = Path.GetFileNameWithoutExtension(getFilters[i]);
                }
                string selectedFilter = getFilters.Contains(FTSettings.FilterFileSource) ? FTSettings.FilterFileSource : getFilters[0];
                ViewFilters = new SourceFilters(getFilters, selectedFilter);
                NotifyPropertyChanged("ViewFilters");
            }
        }
        private void GetFilterPathSource()
        {
            var getFile = OpenFilterDialogue();
            if (getFile == "") return;
            FTSettings.FilterPathSource = Path.GetDirectoryName(getFile);
            FTSettings.FilterFileSource = Path.GetFileNameWithoutExtension(getFile);         
            SaveSettings();
        }
        private void GetFilterPathTarget()
        {
            var getFile = OpenFilterDialogue();
            if (getFile == "") return;
            FTSettings.FilterPathTarget = Path.GetDirectoryName(getFile);
            FTSettings.FilterFileTarget = Path.GetFileNameWithoutExtension(getFile);
            SaveSettings();
        }
        private void LoadSettings()
        {
            var XMLSerializer = new XmlSerializer(typeof(Settings));
            if (!File.Exists("Settings.xml"))
            {
                SaveSettings();
                return;
            }
            using (var fs = File.Open("Settings.xml", FileMode.Open))
            {
                FTSettings=(Settings)XMLSerializer.Deserialize(fs);
            }
            return;
        }
        private void SaveSettings()
        {
            var XMLSerializer = new XmlSerializer(typeof(Settings));
            using (var fs = File.Open("Settings.xml", FileMode.Create))
            {
                XMLSerializer.Serialize(fs, FTSettings);
            }

        }
        #endregion
        public MainScreenViewModel() //Initializing variables for main screen
        {
            TemplatesSource = new TemplateManager(templateFileName);
            FTSettings = new Settings("", "", "", "");
            Templates = LoadTemplates();
            ChosenTemplate = Templates.FirstOrDefault();
            MockSavedTemplates = LoadTemplates();
            ViewFilters = new SourceFilters(new String[0], String.Empty);
            #region Commands
            SaveFilterCommand = new GenericCommand(x => SaveFilter());
            RemoveTemplateCommand = new GenericCommand(x => RemoveTemplate());
            SaveTemplateCommand = new GenericCommand(x => SaveTemplate());
            FilterPathSourceCommand = new GenericCommand(x => { GetFilterPathSource(); RefreshFilterList(); });
            FilterPathTargetCommand = new GenericCommand(x => { GetFilterPathTarget(); NotifyPropertyChanged("FilterTargetName");});
            #endregion
            LoadSettings();
            RefreshFilterList();
        }
    }
}
