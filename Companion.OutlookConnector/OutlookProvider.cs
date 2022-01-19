using System.Diagnostics;
using System.Reflection;
using Microsoft.Office.Interop.Outlook;

namespace Companion.OutlookConnector;

internal class OutlookProvider
{
    private readonly object _lockObject = new();
    private Application? _application;

    public Application GetApplication()
    {
        if (_application == null)
        {
            lock (_lockObject)
            {
                _application = GetApplicationObject();
                ((ApplicationEvents_11_Event)_application).Quit += QuitHandler;
            }
        }

        return _application;
    }

    private void QuitHandler()
    {
        lock (_lockObject)
        {
            ((ApplicationEvents_11_Event)_application).Quit -= QuitHandler;
            _application = null;
        }
    }

    private Application GetApplicationObject()
    {
        Application? application = null;

        // Check whether there is an Outlook process running.
        if (Process.GetProcessesByName("OUTLOOK").Length > 0)
        {

            // If so, use the GetActiveObject method to obtain the process and cast it to an Application object.
            application = new Application();
        }
        else
        {

            // If not, create a new instance of Outlook and sign in to the default profile.
            application = new Application();
            NameSpace? nameSpace = application.GetNamespace("MAPI");
            nameSpace.Logon("", "", Missing.Value, Missing.Value);
            nameSpace = null;
        }

        // Return the Outlook Application object.
        return application;
    }
}