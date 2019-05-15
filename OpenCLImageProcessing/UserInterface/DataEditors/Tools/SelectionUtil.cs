using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using OpenTK;
using Processing;

namespace UserInterface.DataEditors.Tools
{
    public static class SelectionUtil
    {
        public enum SelectionEditAction
        {
            None,
            Move,
            ResizeNW,
            ResizeNE,
            ResizeSE,
            ResizeSW,
            ResizeN,
            ResizeE,
            ResizeS,
            ResizeW
        }

        public static SelectionEditAction ResolveSelectionEditAction(PointF cursorPosition, Rectangle selection, float editRadius)
        {
            return ResolveSelectionEditAction(new Vector2(cursorPosition.X, cursorPosition.Y), ImageSelection.FromRectangle(selection), editRadius);
        }

        public static SelectionEditAction ResolveSelectionEditAction(Vector2 cursorPosition, ImageSelection selection, float editRadius)
        {
            var nw = new Vector2(selection.X0, selection.Y0);
            var sw = new Vector2(selection.X0, selection.Y1);
            var se = new Vector2(selection.X1, selection.Y1);
            var ne = new Vector2(selection.X1, selection.Y0);

            var action = SelectionEditAction.None;

            if (cursorPosition.InRadius(se, editRadius))
                action = SelectionEditAction.ResizeSE;
            else if (cursorPosition.InRadius(ne, editRadius))
                action = SelectionEditAction.ResizeNE;
            else if (cursorPosition.InRadius(nw, editRadius))
                action = SelectionEditAction.ResizeNW;
            else if (cursorPosition.InRadius(sw, editRadius))
                action = SelectionEditAction.ResizeSW;
            else if (cursorPosition.DistanceToSegment(nw, ne).Abs() < editRadius)
                action = SelectionEditAction.ResizeN;
            else if (cursorPosition.DistanceToSegment(ne, se).Abs() < editRadius)
                action = SelectionEditAction.ResizeE;
            else if (cursorPosition.DistanceToSegment(se, sw).Abs() < editRadius)
                action = SelectionEditAction.ResizeS;
            else if (cursorPosition.DistanceToSegment(nw, sw).Abs() < editRadius)
                action = SelectionEditAction.ResizeW;
            else if (selection.GetRectangle().ContainsPoint((int) cursorPosition.X, (int) cursorPosition.Y))
                action = SelectionEditAction.Move;

            return action;
        }

        public static Cursor ResolveCursor(SelectionEditAction action)
        {
            if (action == SelectionUtil.SelectionEditAction.ResizeNW || action == SelectionUtil.SelectionEditAction.ResizeSE)
                return Cursors.SizeNWSE;
            
            if (action == SelectionUtil.SelectionEditAction.ResizeNE || action == SelectionUtil.SelectionEditAction.ResizeSW)
                return Cursors.SizeNESW;
            
            if (action == SelectionUtil.SelectionEditAction.ResizeN || action == SelectionUtil.SelectionEditAction.ResizeS)
                return Cursors.SizeNS;
            
            if (action == SelectionUtil.SelectionEditAction.ResizeW || action == SelectionUtil.SelectionEditAction.ResizeE)
                return Cursors.SizeWE;
            
            if (action == SelectionUtil.SelectionEditAction.Move)
                return Cursors.SizeAll;

            return Cursors.Default;
        }

        public static void ApplySelectionTransformation(SelectionEditAction action, PointF cursorPosition, PointF previousCursorPosition, ref Rectangle selection)
        {
            var imageSelection = ImageSelection.FromRectangle(selection);

            ApplySelectionTransformation(action, cursorPosition.ToVector2(), previousCursorPosition.ToVector2(), imageSelection);

            selection.X = imageSelection.X0;
            selection.Y = imageSelection.Y0;
            selection.Width = imageSelection.Width;
            selection.Height = imageSelection.Height;
        }

        public static void ApplySelectionTransformation(SelectionEditAction action, Vector2 cursorPosition, Vector2 previousCursorPosition, ImageSelection selection)
        {
            switch (action)
            {
                case SelectionEditAction.ResizeNW:
                    selection.X0 = (int) cursorPosition.X;
                    selection.Y0 = (int) cursorPosition.Y;
                    break;
                case SelectionEditAction.ResizeSW:
                    selection.X0 = (int) cursorPosition.X;
                    selection.Y1 = (int) cursorPosition.Y;
                    break;
                case SelectionEditAction.ResizeSE:
                    selection.X1 = (int) cursorPosition.X;
                    selection.Y1 = (int) cursorPosition.Y;
                    break;
                case SelectionEditAction.ResizeNE:
                    selection.X1 = (int) cursorPosition.X;
                    selection.Y0 = (int) cursorPosition.Y;
                    break;
                case SelectionEditAction.ResizeN:
                    selection.Y0 = (int) cursorPosition.Y;
                    break;
                case SelectionEditAction.ResizeS:
                    selection.Y1 = (int) cursorPosition.Y;
                    break;
                case SelectionEditAction.ResizeW:
                    selection.X0 = (int) cursorPosition.X;
                    break;
                case SelectionEditAction.ResizeE:
                    selection.X1 = (int) cursorPosition.X;
                    break;
                case SelectionEditAction.Move:
                    selection.MoveBy(cursorPosition - previousCursorPosition);
                    break;
            }
        }

        public static Bitmap ExtractSelection(Bitmap image, Rectangle selection)
        {
            var result = new Bitmap(selection.Width, selection.Height);
            var graphics = Graphics.FromImage(result);
            graphics.DrawImage(image, new Rectangle(Point.Empty, selection.Size), selection, GraphicsUnit.Pixel);

            return result;
        }
    }
}