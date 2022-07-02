using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
//using DS.RevitLib.Utils.MEP.Creator;
using System;
using System.Windows.Input;
using Revit.Async;
using System.Threading.Tasks;

namespace RevitTaskExample
{
    public class TestMainViewModel1
    {
        private RevitTask _revitTask = new RevitTask();
        private Document _Doc;
        private UIDocument _Uidoc;

        public TestMainViewModel1(UIDocument Uidoc, Document Doc)
        {
            _Doc = Doc;
            _Uidoc = Uidoc;

            RunLongRevit = new UiCommand(Operation2);
        }

        public ICommand RunLongRevit { get; set; }

        private void MoveElement1()
        {
            Reference reference = _Uidoc.Selection.PickObject(ObjectType.Element, "Select element that will be checked for intersection with all elements");
            Element element = _Doc.GetElement(reference);

            Move(element, new XYZ(1, 0, 0));
        }

        private void MoveElement2()
        {
            Reference reference = _Uidoc.Selection.PickObject(ObjectType.Element, "Select element that will be checked for intersection with all elements");
            Element element = _Doc.GetElement(reference);

            Move(element, new XYZ(-2, 0, 0));
        }

        private async Task Mover1()
        {
            try
            {
                await RevitTask.RunAsync((uiApp) => MoveElement1());
            }
            catch (Exception e)
            {
                TaskDialog.Show("Debug", e.Message);
            }
        }

        private async Task Mover2()
        {
            try
            {
                await RevitTask.RunAsync((uiApp) => MoveElement2());
            }
            catch (Exception e)
            {
                TaskDialog.Show("Debug", e.Message);
            }
        }

        private void Operation()
        {
            Mover1();
            TaskDialog.Show("Revit", "Test");
            Mover2();
        }

        private async void Operation1()
        {
          await RevitTask.RunAsync(
           app =>
           {
               Mover1();
               TaskDialog.Show("Revit", "Test1");
               TaskDialog.Show("Revit", "Test2");
               TaskDialog.Show("Revit", "Test3");
               Mover2();
           });         
        }

        private async void Operation2()
        {
            await Mover1();
            TaskDialog.Show("Revit", "Test");
            await Mover2();
        }



        /// <summary>
        /// Move MEPCurve by specified vector.
        /// </summary>
        /// <param name="vector"></param>
        /// <returns>Return moved MEPCurve.</returns>
        public Element Move(Element element, XYZ vector)
        {
            using (Transaction transNew = new Transaction(_Doc, "move"))
            {
                try
                {
                    transNew.Start();
                    ElementTransformUtils.MoveElement(_Doc, element.Id, vector);
                }
                catch (Exception e)
                {
                    TaskDialog.Show("Error", e.Message);
                }
                if (transNew.HasStarted())
                {
                    transNew.Commit();
                }
            }

            return element;
        }

    }
}