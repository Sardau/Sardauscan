#region COPYRIGHT
/**
 * This file is part of Sardauscan by Fabio Ferretti, licensed under the CC-BY-NC-SA 4.0 Licence.
 * You can find the original code in this GitHub repository: https://github.com/Sardau/Sardauscan
 */
# endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Sardauscan.Core.ProcessingTask
{
	/// <summary>
	/// Remove Empty ScanLines
	/// </summary>
    [Browsable(false)]
    public class LinesRemoveEmpty : AbstractLineTask
    {
			/// <summary>
			/// Do the work
			/// </summary>
			/// <param name="source"></param>
			/// <returns></returns>
        public override ScanLine DoTask(ScanLine source)
        {
            if (source.Count != 0)
                return source;
            return null;
        }
			/// <summary>
			/// Name of the task
			/// </summary>
        public override string Name
        {
            get { return "Remove Empty"; }
        }
    }
}
