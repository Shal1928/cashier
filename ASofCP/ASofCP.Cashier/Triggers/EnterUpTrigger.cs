using System;
using System.Windows.Input;
using ASofCP.Cashier.Helpers;
using EventTrigger = System.Windows.Interactivity.EventTrigger;

namespace ASofCP.Cashier.Triggers
{
    public class EnterUpTrigger : EventTrigger
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            EventName = "KeyUp";
        }

        protected override void OnEvent(EventArgs eventArgs)
        {
            var keyEventArgs = eventArgs as KeyEventArgs;
            if (keyEventArgs.IsNull() || keyEventArgs.Key != Key.Enter) return;

            base.OnEvent(eventArgs);
        }
    }
}
