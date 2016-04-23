using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GraphSharp.Controls;
using QuickGraph;

namespace Stratosphere.MachineLearning.Studio.Graphs
{

    [DebuggerDisplay("{ID}-{IsInput}")]
    public class PocVertex
    {
        public string ID { get; private set; }
        public bool IsInput { get; private set; }

        public PocVertex(string id, bool isInput)
        {
            ID = id;
            IsInput = isInput;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", ID, IsInput);
        }
    }

    /// <summary>
    /// A simple identifiable edge.
    /// </summary>
    [DebuggerDisplay("{Source.ID} -> {Target.ID}")]
    public class PocEdge : Edge<PocVertex>
    {
        public string ID
        {
            get;
            private set;
        }

        public PocEdge(string id, PocVertex source, PocVertex target)
            : base(source, target)
        {
            ID = id;
        }
    }

    public class PocGraph : BidirectionalGraph<PocVertex, PocEdge>
    {
        public PocGraph() { }

        public PocGraph(bool allowParallelEdges)
            : base(allowParallelEdges)
        { }

        public PocGraph(bool allowParallelEdges, int vertexCapacity)
            : base(allowParallelEdges, vertexCapacity)
        { }
    }

    public class PocGraphLayout : GraphLayout<PocVertex, PocEdge, PocGraph> { }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Data
        private string layoutAlgorithmType;
        private PocGraph graph;
        private List<String> layoutAlgorithmTypes = new List<string>();
        #endregion

        #region Ctor
        public MainWindowViewModel()
        {
            Graph = new PocGraph(true);

            List<PocVertex> existingVertices = new List<PocVertex>();
            existingVertices.Add(new PocVertex("1", true)); //0
            existingVertices.Add(new PocVertex("x1", true)); //1
            existingVertices.Add(new PocVertex("x2", true)); //2
            existingVertices.Add(new PocVertex(" ", false)); //3
            existingVertices.Add(new PocVertex("Output", false)); //4

            foreach (PocVertex vertex in existingVertices)
                Graph.AddVertex(vertex);

            //add some edges to the graph
            AddNewGraphEdge(existingVertices[0], existingVertices[3]);
            AddNewGraphEdge(existingVertices[1], existingVertices[3]);
            AddNewGraphEdge(existingVertices[2], existingVertices[3]);

            AddNewGraphEdge(existingVertices[3], existingVertices[4]);
            
            //Add Layout Algorithm Types
            layoutAlgorithmTypes.Add("BoundedFR");
            layoutAlgorithmTypes.Add("Circular");
            layoutAlgorithmTypes.Add("CompoundFDP");
            layoutAlgorithmTypes.Add("EfficientSugiyama");
            layoutAlgorithmTypes.Add("FR");
            layoutAlgorithmTypes.Add("ISOM");
            layoutAlgorithmTypes.Add("KK");
            layoutAlgorithmTypes.Add("LinLog");
            layoutAlgorithmTypes.Add("Tree");

            //Pick a default Layout Algorithm Type
            LayoutAlgorithmType = "LinLog";

        }
        #endregion

        #region Private Methods
        private PocEdge AddNewGraphEdge(PocVertex from, PocVertex to)
        {
            string edgeString = string.Format("{0}-{1} Connected", from.ID, to.ID);

            PocEdge newEdge = new PocEdge(edgeString, from, to);
            Graph.AddEdge(newEdge);
            return newEdge;
        }

        #endregion

        #region Public Properties

        public List<String> LayoutAlgorithmTypes
        {
            get { return layoutAlgorithmTypes; }
        }

        public string LayoutAlgorithmType
        {
            get { return layoutAlgorithmType; }
            set
            {
                layoutAlgorithmType = value;
                NotifyPropertyChanged("LayoutAlgorithmType");
            }
        }

        public PocGraph Graph
        {
            get { return graph; }
            set
            {
                graph = value;
                NotifyPropertyChanged("Graph");
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
