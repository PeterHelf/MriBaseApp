using Android.Views;
using MriBase.App.Base.Events.Arguments;
using MriBase.App.Dog.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportEffect(typeof(TouchEffectWithCoordinates), "TouchEffectWithCoordinates")]
namespace MriBase.App.Dog.Droid
{
    public class TouchEffectWithCoordinates : PlatformEffect
    {
        private Android.Views.View view;
        private Element formsElement;
        private Base.Custom.TouchEffectWithCoordinates libTouchEffect;
        private Func<double, double> fromPixels;
        private int[] twoIntArray = new int[2];

        private static Dictionary<Android.Views.View, TouchEffectWithCoordinates> viewDictionary =
            new Dictionary<Android.Views.View, TouchEffectWithCoordinates>();

        protected override void OnAttached()
        {
            // Get the Android View corresponding to the Element that the effect is attached to
            view = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the .NET Standard library
            Base.Custom.TouchEffectWithCoordinates touchEffect =
                (Base.Custom.TouchEffectWithCoordinates)Element.Effects.
                    FirstOrDefault(e => e is Base.Custom.TouchEffectWithCoordinates);

            if (touchEffect != null && view != null)
            {
                viewDictionary.Add(view, this);

                formsElement = Element;

                libTouchEffect = touchEffect;

                // Save fromPixels function
                fromPixels = view.Context.FromPixels;

                // Set event handler on View
                view.Touch += OnTouch;
            }
        }

        protected override void OnDetached()
        {
            if (viewDictionary.ContainsKey(view))
            {
                viewDictionary.Remove(view);
                view.Touch -= OnTouch;
            }
        }

        private void OnTouch(object sender, Android.Views.View.TouchEventArgs args)
        {
            // Two object common to all the events
            Android.Views.View senderView = sender as Android.Views.View;
            MotionEvent motionEvent = args.Event;

            // Get the pointer index
            int pointerIndex = motionEvent.ActionIndex;

            // Get the id that identifies a finger over the course of its progress
            int id = motionEvent.GetPointerId(pointerIndex);


            senderView.GetLocationOnScreen(twoIntArray);
            Point screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                  twoIntArray[1] + motionEvent.GetY(pointerIndex));


            // Use ActionMasked here rather than Action to reduce the number of possibilities
            switch (args.Event.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    FireEvent(this, id, TouchActionType.Pressed, screenPointerCoords, true);
                    break;

                case MotionEventActions.Move:
                    // Multiple Move events are bundled, so handle them in a loop
                    for (pointerIndex = 0; pointerIndex < motionEvent.PointerCount; pointerIndex++)
                    {
                        id = motionEvent.GetPointerId(pointerIndex);

                        senderView.GetLocationOnScreen(twoIntArray);

                        screenPointerCoords = new Point(twoIntArray[0] + motionEvent.GetX(pointerIndex),
                                                        twoIntArray[1] + motionEvent.GetY(pointerIndex));

                        FireEvent(this, id, TouchActionType.Moved, screenPointerCoords, true);
                    }
                    break;

                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    FireEvent(this, id, TouchActionType.Released, screenPointerCoords, false);
                    break;

                case MotionEventActions.Cancel:

                    FireEvent(this, id, TouchActionType.Cancelled, screenPointerCoords, false);
                    break;
            }
        }

        void FireEvent(TouchEffectWithCoordinates touchEffect, int id, TouchActionType actionType, Point pointerLocation, bool isInContact)
        {
            // Get the method to call for firing events
            Action<Element, CalibratedTouchActionEventArgs> onTouchAction = touchEffect.libTouchEffect.OnTouchAction;

            double x;
            double y;

            // Get the location of the pointer within the view
            if (touchEffect.libTouchEffect.GetCoordinatesRelativeToView)
            {
                touchEffect.view.GetLocationOnScreen(twoIntArray);
                x = pointerLocation.X - twoIntArray[0];
                y = pointerLocation.Y - twoIntArray[1];
            }
            else
            {
                x = pointerLocation.X;
                y = pointerLocation.Y;
            }

            Point point = new Point(fromPixels(x), fromPixels(y));

            // Call the method
            onTouchAction(touchEffect.formsElement,
                new CalibratedTouchActionEventArgs(id, actionType, point, isInContact));
        }
    }
}