using System.ComponentModel;
using System.Runtime.CompilerServices;
using SnakesOfLife.Annotations;

namespace SnakesOfLife.Models
{
    public class GrassCell : INotifyPropertyChanged
    {
        private int _neededAliveNeighborsTurnsToGrow;

        public int NeededAliveNeighborsTurnsToGrow
        {
            get { return _neededAliveNeighborsTurnsToGrow; }
            private set
            {
                if (value == _neededAliveNeighborsTurnsToGrow) return;
                _neededAliveNeighborsTurnsToGrow = value;
                OnPropertyChanged();
                OnPropertyChanged("IsAlive");
            }
        }

        public bool IsAlive
        {
            get { return NeededAliveNeighborsTurnsToGrow == 0; }
        }

        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }

        public GrassCell()
        {
        }

        public GrassCell(int rowIndex, int columnIndex)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public GrassCell(int rowIndex, int columnIndex, bool alive)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
        }

        public bool EnteredBySnake()
        {
            if (!IsAlive)
            {
                return false;
            }

            NeededAliveNeighborsTurnsToGrow = Params.Current.NeededAliveNeighborsTurnsToGrow;

            return true;
        }

        public void UpdateGrowth(int aliveNeighborsAtTurn)
        {
            if (IsAlive)
            {
                return;
            }

            NeededAliveNeighborsTurnsToGrow -= aliveNeighborsAtTurn;

            if (NeededAliveNeighborsTurnsToGrow <= 0)
            {
                NeededAliveNeighborsTurnsToGrow = 0;
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
