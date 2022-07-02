using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Revit.Async;
using System.Threading;

namespace RevitTaskExample
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class StartCommand :
        IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            Autodesk.Revit.ApplicationServices.Application application = uiapp.Application;

            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uiapp.ActiveUIDocument.Document;
            RevitTask.Initialize(uiapp);

            var id = Thread.CurrentThread.ManagedThreadId;

            //var viewModel = new TestMainViewModel(uidoc, doc);
            var viewModel = new TestMainViewModel1(uidoc, doc);
            var mainWindow = new MainWindow(viewModel);
            //var viewModel = new MainViewModel();
            //var mainWindow = new MainWindow();


            mainWindow.Show();

            return Result.Succeeded;
        }
    }
}