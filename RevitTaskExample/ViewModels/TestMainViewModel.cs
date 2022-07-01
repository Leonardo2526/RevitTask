using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
//using DS.RevitLib.Utils.MEP.Creator;
using System;
using System.Windows.Input;

namespace RevitTaskExample
{
    public class TestMainViewModel
    {
        private RevitTask _revitTask = new RevitTask();
        private Document _Doc;
        private UIDocument _Uidoc;

        public TestMainViewModel(UIDocument Uidoc, Document Doc)
        {
            _Doc = Doc;
            _Uidoc = Uidoc;

            RunLongRevit = new UiCommand(PlaceInstances);
        }

        public ICommand RunLongRevit { get; set; }

        private void MoveFamilyInstance()
        {
            Reference reference = _Uidoc.Selection.PickObject(ObjectType.Element, "Select element that will be checked for intersection with all elements");
            Element element = _Doc.GetElement(reference);

            Move(element, new XYZ(1, 0, 0));
        }

        private async void PlaceInstances()
        {
            try
            {
                await _revitTask.Run((uiApp) => MoveFamilyInstance());
            }
            catch (Exception e)
            {
                TaskDialog.Show("Debug", e.Message);
            }
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