#region License

/*
Camera_NET - Camera wrapper for directshow for .NET
Copyright (C) 2013
https://github.com/free5lot/Camera_Net

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3.0 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU LesserGeneral Public 
License along with this library. If not, see <http://www.gnu.org/licenses/>.
*/

#endregion

namespace Camera_NET
{
    #region Using directives

    using System;
    using System.Runtime.InteropServices;

    #endregion

    
    /// <summary>
    /// NativeMethodes is a wrapper for native methods.
    /// </summary>
    /// 
    /// <author> free5lot (free5lot@yandex.ru) </author>
    /// <version> 2013.10.16 </version>
    public sealed class NativeMethodes
    {
    // Graphics.GetHdc() have several "bugs" detailed here : 
    // http://support.microsoft.com/default.aspx?scid=kb;en-us;311221
    // (see case 2) So we have to play with old school GDI...
    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

    [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
    public static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

    //A (modified) definition of OleCreatePropertyFrame found here: http://groups.google.no/group/microsoft.public.dotnet.languages.csharp/browse_thread/thread/db794e9779144a46/55dbed2bab4cd772?lnk=st&q=[DllImport(%22olepro32.dll%22)]&rnum=1&hl=no#55dbed2bab4cd772
    [DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern int OleCreatePropertyFrame(
        IntPtr hwndOwner,
        int x,
        int y,
        [MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
        int cObjects,
        [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] 
        ref object ppUnk,
        int cPages,
        IntPtr lpPageClsID,
        int lcid,
        int dwReserved,
        IntPtr lpvReserved);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CreateFile(
         [MarshalAs(UnmanagedType.LPTStr)] string filename,
         [MarshalAs(UnmanagedType.U4)] System.IO.FileAccess access,
         [MarshalAs(UnmanagedType.U4)] System.IO.FileShare share,
         IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
         [MarshalAs(UnmanagedType.U4)] System.IO.FileMode creationDisposition,
         [MarshalAs(UnmanagedType.U4)] System.IO.FileAttributes flagsAndAttributes,
         IntPtr templateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    public static extern bool SetFilePointerEx(IntPtr hFile, long liDistanceToMove,
       IntPtr lpNewFilePointer, uint dwMoveMethod);

    // SetFilePointerEx uses flag to append
    public const uint FILE_END = 2;
  }
}
