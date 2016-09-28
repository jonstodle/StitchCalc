using System;
namespace StitchCalc.iOS
{
	public class LinkerPleaseInclude
	{
		public void Include()
		{
			throw new Exception(new System.ComponentModel.ReferenceConverter(typeof(void)).ToString());
		}
	}
}

