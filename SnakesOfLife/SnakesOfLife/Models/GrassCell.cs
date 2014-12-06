using System.ComponentModel;

namespace SnakesOfLife.Models
{
    public class GrassCell : INotifyPropertyChanged
    {
        public int NeededAliveNeighborsTurnsToGrow { get; private set; }

        //Private internal 'alive' boolean for cell.
        private bool _isAlive = false;


        /* Public boolean used to determine if the cell is alive or dead. 
         * 
         * Contains a PropertyChanged object to automatically inform the UI when the boolean
         * changes, so that the grid can be updated as required.
         */
        public bool IsAlive
        {
            get
            {
                //Return the private internal 'alive' boolean.
                return _isAlive;
            }

            set
            {
                //Set the internal 'alive' boolean.
                _isAlive = value;

                //Inform the UI of the changes to the boolean (i.e. Inform the Observer of a change in state)
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsAlive"));
                }
            }
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
            NeededAliveNeighborsTurnsToGrow = Params.Instance.NeededAliveNeighborsTurnsToGrow;
        }

        public GrassCell(int rowIndex, int columnIndex, bool alive)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            IsAlive = alive;
            NeededAliveNeighborsTurnsToGrow = Params.Instance.NeededAliveNeighborsTurnsToGrow;
        }

        public bool EnteredBySnake()
        {
            if (!IsAlive)
            {
                return false;
            }

            NeededAliveNeighborsTurnsToGrow = Params.Instance.NeededAliveNeighborsTurnsToGrow;

            return true;
        }

        public void UpdateGrowth(int aliveNeighborsAtTurn)
        {
            //if (IsAlive)
            //{
            //    return;
            //}

            NeededAliveNeighborsTurnsToGrow -= aliveNeighborsAtTurn;

            if (NeededAliveNeighborsTurnsToGrow <= 0)
            {
                NeededAliveNeighborsTurnsToGrow = 0;
                IsAlive = true;
            }
            else
            {
                IsAlive = false;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
