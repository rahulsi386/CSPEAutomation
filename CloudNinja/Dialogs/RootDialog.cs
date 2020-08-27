using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;

namespace CloudNinja.Dialogs
{
    public class RootDialog<T> : ComponentDialog where T : Dialog
    {
        public RootDialog()
        {

        }
    }
}
