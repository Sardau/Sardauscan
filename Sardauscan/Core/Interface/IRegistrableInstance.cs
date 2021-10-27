#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sardauscan.Core.Interface
{
	/// <summary>
	/// IRegistrableInstance
	/// </summary>
    interface IRegistrableInstance
    {
			 /// <summary>
			 /// Function called when instance is registred
			 /// </summary>
			 /// <returns></returns>
        bool OnRegister();
    }
}
