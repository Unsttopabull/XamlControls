using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Frost.XamlControls {

    /// <Remarks>As a side effect ClippingBorder will surpress any databinding or animation of its childs UIElement.Clip property until the child is removed from ClippingBorder</Remarks>
    public class ClippingBorder : Border {
        private readonly RectangleGeometry _clipRect = new RectangleGeometry();
        private object _oldClip;

        public override UIElement Child {
            get { return base.Child; }
            set {
                if (Child == value) {
                    return;
                }

                if (Child != null) {
                    // Restore original clipping
                    Child.SetValue(ClipProperty, _oldClip);
                }

                _oldClip = value != null ? value.ReadLocalValue(ClipProperty) : null;

                base.Child = value;
            }
        }

        protected override void OnRender(DrawingContext dc) {
            OnApplyChildClip();
            base.OnRender(dc);
        }

        protected virtual void OnApplyChildClip() {
            UIElement child = Child;
            if (child == null) {
                return;
            }

            _clipRect.RadiusX = _clipRect.RadiusY = Math.Max(0.0, CornerRadius.TopLeft - (BorderThickness.Left * 0.5));
            _clipRect.Rect = new Rect(Child.RenderSize);
            child.Clip = _clipRect;
        }
    }

}