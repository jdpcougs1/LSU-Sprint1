namespace LSU_App
{
    using System;
    using System.Windows.Forms;

    namespace LSU_App
    {
        /// <summary>
        /// Main entry point for the LSU_App Windows Forms application.
        /// This class contains the application startup logic and is responsible for
        /// initializing the Windows Forms runtime and launching the primary user interface.
        /// 
        /// The application uses the traditional Windows Forms architecture with Form1
        /// serving as the main application window. This is a single-threaded apartment (STA)
        /// application suitable for desktop GUI interactions.
        internal static class Program
        {
            /// <summary>
            /// Sets the application to use Single Threaded Apartment (STA) model.
            /// This attribute is required for Windows Forms applications to properly handle
            /// COM interop, clipboard operations, drag-and-drop functionality, and other
            /// Windows GUI features that require STA threading.
            /// 
            /// Without this attribute, the application may experience issues with:
            /// - File dialogs and common dialogs
            /// - Clipboard operations
            /// - OLE drag and drop
            /// - Some third-party controls
            /// </summary>
            [STAThread]
            static void Main()
            {
                // Initialize the Windows Forms application configuration
                // This method sets up DPI awareness, visual styles, and other modern Windows Forms features
                ApplicationConfiguration.Initialize();
                // Start the main message loop with Form1 as the primary application window
                // This call blocks until the main form is closed or Application.Exit() is called
                // Form1 should contain the main user interface for the LSU admissions application
                // The message loop handles all Windows messages, events, and user interactions
                Application.Run(new Form1());

                // Error handling and cleanup code will go here

            }
        }
    }
}