using System;
using System.Runtime.InteropServices;
using SNAGITLib;

namespace SnagitImgur.Plugin
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("EDA9F1DD-85B9-44F6-9D3C-125B2DD1109D")]
    public class Package : MarshalByRefObject, IPackageSetup
    {
        public void Install()
        {
        }

        public void Uninstall()
        {
        }
    }
}