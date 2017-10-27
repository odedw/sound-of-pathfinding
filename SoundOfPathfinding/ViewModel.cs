﻿using SoundOfPathfinding.Generators;
using SoundOfPathfinding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SoundOfPathfinding
{
    public class ViewModel : INotifyPropertyChanged
    {
        //public Dispatcher Dispatcher { get; set; }

        public ICommand GenerateCommand { get; set; }


        private Maze _maze;
        public Maze Maze
        {
            get { return _maze; }
            set
            {
                if (_maze == value) return;
                _maze = value;
                RaisePropertyChanged("Maze");
            }
        }

        public ViewModel(int rows, int cols)
        {
            Maze = new Maze(rows, cols);
            var rand = new Random();
            
            GenerateCommand = new RelayCommand(o =>
            {
                var generator = new DepthFirstSearchGenerator(Maze);
                while (generator.NextStep())
                {

                }
            });

        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChanged?.Invoke (this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
