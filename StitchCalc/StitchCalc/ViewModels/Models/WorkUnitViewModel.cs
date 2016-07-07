using StitchCalc.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.ViewModels.Models
{
    public class WorkUnitViewModel : ViewModelBase
    {
		readonly WorkUnit model;

		public WorkUnitViewModel(WorkUnit model)
		{
			this.model = model;
		}

		public WorkUnit Model => model;

		public double ChargePerHour => model.Charge * 60;

		public double TotalCharge => model.Charge * model.Minutes;

		public int Minutes => model.Minutes;

		public double Hours => model.Minutes / 60;

		public TimeSpan Time => TimeSpan.Zero + TimeSpan.FromMinutes(model.Minutes);
    }
}
