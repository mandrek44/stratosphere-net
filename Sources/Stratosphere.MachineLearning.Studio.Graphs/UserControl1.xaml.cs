
using System.Windows.Controls;
using Graphviz4Net.Graphs;

namespace Stratosphere.MachineLearning.Studio.Graphs
{

    
    public class MainWindowViewModel
    {
        public Graph<Person> Graph { get; set; }

        public MainWindowViewModel()
        {
            
            Graph = new Graph<Person>();
            var bias = new Person { Name = "1", };
            var a = new Person { Name = "a", };
            var b = new Person { Name = "b",  };
            var neuron = new Person { Name = "S(t)", };
            var neuron2 = new Person { Name = "S(t)", };
            var output = new Person { Name = "Output", };

            Graph.AddVertex(a);
            Graph.AddVertex(b);
            Graph.AddVertex(bias);
            Graph.AddVertex(neuron);
            Graph.AddVertex(neuron2);
            Graph.AddVertex(output);
            Connect(neuron, bias, a, b, output);
            Connect(neuron2, bias, a, b, output);
        }

        private void Connect(Person neuron, Person bias, Person a, Person b, Person output)
        {
            Graph.AddEdge(new Edge<Person>(bias, neuron, new DiamondArrow()) {Label = "\\-30"});
            Graph.AddEdge(new Edge<Person>(a, neuron, new DiamondArrow()) {Label = "20"});
            Graph.AddEdge(new Edge<Person>(b, neuron, new DiamondArrow()) {Label = "20"});
            Graph.AddEdge(new Edge<Person>(neuron, output, new DiamondArrow()) {Label = ""});
        }
    }

    public class DiamondArrow
    {
    }

    public class Person
    {
        public string Name { get; set; }
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
