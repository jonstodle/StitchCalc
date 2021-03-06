﻿using ReactiveUI;
using StitchCalc.Models;
using StitchCalc.Services;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace StitchCalc.ViewModels
{
    public class MaterialFormViewModel : ViewModelBase
    {
        public MaterialFormViewModel(Material material = null)
        {
            _material = material;
            Name = _material?.Name;
            Width = _material?.Width.ToString();
            Price = _material?.Price.ToString();
            Notes = _material?.Notes;

            _save = ReactiveCommand.CreateFromObservable(
                () => SaveImpl(),
                this.WhenAnyValue(a => a.Name, b => b.Width, c => c.Price, (a, b, c) =>
                {
                    double amnt, prc = default(double);
                    return !string.IsNullOrWhiteSpace(a)
                    && !string.IsNullOrWhiteSpace(b)
                    && !string.IsNullOrWhiteSpace(b)
                    && double.TryParse(b, out amnt)
                    && double.TryParse(c, out prc)
                    && amnt > 0
                    && prc > 0;
                }));
        }

        public ReactiveCommand Save => _save;

        public string PageTitle => _material == null ? "Add Material" : "Edit Material";

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public string Width
        {
            get { return _width; }
            set { this.RaiseAndSetIfChanged(ref _width, value); }
        }

        public string Price
        {
            get { return _price; }
            set { this.RaiseAndSetIfChanged(ref _price, value); }
        }

        public string Notes
        {
            get { return _notes; }
            set { this.RaiseAndSetIfChanged(ref _notes, value); }
        }

        public Material Material
        {
            get { return _material; }
            set { this.RaiseAndSetIfChanged(ref _material, value); }
        }




        private IObservable<Unit> SaveImpl() => Observable.StartAsync(async () =>
        {
            var material = new Material
            {
                Name = Name,
                Width = double.Parse(Width),
                Price = double.Parse(Price),
                Notes = _notes
            };
            if (_material != null) material.Id = _material.Id;

            DBService.Write(realm => realm.Add(material, true));

            await NavigationService.GoBack();
        });



        private readonly ReactiveCommand<Unit, Unit> _save;
        private string _name;
        private string _width;
        private string _price;
        private string _notes;
        private Material _material;
    }
}
