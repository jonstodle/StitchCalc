using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services.DataServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace StitchCalc.ViewModels.Models
{
    public class WorkUnitViewModel : ViewModelBase
    {
		readonly WorkUnit model;
		readonly ReactiveCommand<object> delete;

		public WorkUnitViewModel(WorkUnit workUnit)
		{
			model = workUnit;

			delete = ReactiveCommand.Create();
			delete
				.Subscribe(_ => DataService.Current.Remove(model));
		}

		public WorkUnit Model => model;

		public ReactiveCommand<object> Delete => delete;

		public string Name => model.Name;

		public double ChargePerHour => model.Charge * 60;

		public double TotalCharge => model.Charge * model.Minutes;

		public int Minutes => model.Minutes;

		public double Hours => model.Minutes / 60;

		public TimeSpan Time => TimeSpan.Zero + TimeSpan.FromMinutes(model.Minutes);
    }
}
