using MriBase.App.Base.Events.Arguments;
using MriBase.App.Base.Events.Handlers;
using Xamarin.Forms;

namespace MriBase.App.Base.Custom
{
    public class TouchEffectWithCoordinates : RoutingEffect
    {
        public event CalibratedTouchActionEventHandler TouchAction;

        public TouchEffectWithCoordinates() : base("MriBase.App.TouchEffectWithCoordinates")
        {
        }

        public bool GetCoordinatesRelativeToView { set; get; }

        public void OnTouchAction(Element element, CalibratedTouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}
