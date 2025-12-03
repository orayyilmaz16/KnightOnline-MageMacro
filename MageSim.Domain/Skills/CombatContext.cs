using MageSim.Domain.Events;
using MageSim.Domain.States;
using System;
using System.ComponentModel;

namespace MageSim.Domain.Skills
{
    public sealed class CombatContext : INotifyPropertyChanged
    {
        private int _mana;

        public int Mana
        {
            get { return _mana; }
            set
            {
                if (_mana != value)
                {
                    _mana = value;
                    OnPropertyChanged(nameof(Mana));
                }
            }
        }

        public bool TargetInRange { get; set; }
        public bool TargetAlive { get; set; }
        public MageState State { get; set; } = MageState.Idle;

        // Nullable event yerine klasik tanım
        public event Action<CombatEvent> OnEvent;

        public void Emit(CombatEvent ev)
        {
            var handler = OnEvent;
            if (handler != null)
            {
                handler(ev);
            }
        }

        // INotifyPropertyChanged implementasyonu
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

