using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MageSim.Application.Simulation;
using MageSim.Domain.Events;

namespace MageSim.Presentation.ViewModels
{
    public sealed class ClientViewModel : INotifyPropertyChanged
    {
        public string Id { get; }
        public ObservableCollection<SkillViewModel> Skills { get; } = new ObservableCollection<SkillViewModel>();

        private int _mana;
        private string _state = "Idle";

        // Artık setter public → TwoWay binding destekler
        public int Mana
        {
            get => _mana;
            set
            {
                if (_mana != value)
                {
                    _mana = value;
                    OnPropertyChanged(nameof(Mana));
                }
            }
        }

        public string State
        {
            get => _state;
            set
            {
                if (_state != value)
                {
                    _state = value;
                    OnPropertyChanged(nameof(State));
                }
            }
        }

        public ClientViewModel(string id) { Id = id; }

        public void Bind(DummyClient client)
        {
            Mana = client.Context.Mana;
            client.Context.OnEvent += ev =>
            {
                if (ev.Type == CombatEventType.StateChange)
                    State = ev.Payload;
                else if (ev.Type == CombatEventType.Cast)
                    Mana = client.Context.Mana;
            };

            foreach (var s in client.Skills)
            {
                Skills.Add(new SkillViewModel
                {
                    Name = s.Name,
                    Key = s.Key,
                    CooldownMs = (int)s.Cooldown.TotalMilliseconds,
                    Mana = s.ManaCost,
                    Condition = s.ConditionDsl
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
