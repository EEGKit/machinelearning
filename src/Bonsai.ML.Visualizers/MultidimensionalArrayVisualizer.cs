using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using Bonsai;
using Bonsai.Design;
using Bonsai.ML.Visualizers;
using Bonsai.ML.LinearDynamicalSystems;
using Bonsai.ML.LinearDynamicalSystems.LinearRegression;
using System.Drawing;
using System.Reactive;
using Bonsai.Reactive;
using Bonsai.Expressions;
using OxyPlot;

[assembly: TypeVisualizer(typeof(MultidimensionalArrayVisualizer), Target = typeof(double[,]))]

namespace Bonsai.ML.Visualizers
{

    /// <summary>
    /// Provides a type visualizer to display the state components of a Kalman Filter kinematics model.
    /// </summary>
    public class MultidimensionalArrayVisualizer : DialogTypeVisualizer
    {
        /// <summary>
        /// Gets or sets the selected index of the color palette to use.
        /// </summary>
        public int PaletteSelectedIndex { get; set; }

        /// <summary>
        /// Gets or sets the selected index of the render method to use.
        /// </summary>
        public int RenderMethodSelectedIndex { get; set; }

        private HeatMapSeriesOxyPlotBase Plot;

        /// <inheritdoc/>
        public override void Load(IServiceProvider provider)
        {
            Plot = new HeatMapSeriesOxyPlotBase(PaletteSelectedIndex, RenderMethodSelectedIndex)
            {
                Dock = DockStyle.Fill,
            };

            Plot.PaletteComboBoxValueChanged += PaletteIndexChanged;
            Plot.RenderMethodComboBoxValueChanged += RenderMethodIndexChanged;

            var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
            if (visualizerService != null)
            {
                visualizerService.AddControl(Plot);
            }
        }

        /// <inheritdoc/>
        public override void Show(object value)
        {
            var mdarray = (double[,])value;
            var shape = new int[] {mdarray.GetLength(0), mdarray.GetLength(1)};

            Plot.UpdateHeatMapSeries(
                -0.5,
                shape[0] - 0.5,
                -0.5,
                shape[1] - 0.5,
                mdarray
            );

            Plot.UpdatePlot();
        }

        /// <inheritdoc/>
        public override void Unload()
        {
            if (!Plot.IsDisposed)
            {
                Plot.Dispose();
            }
        }

        private void PaletteIndexChanged(object sender, EventArgs e)
        {
            PaletteSelectedIndex = Plot.PaletteComboBox.SelectedIndex;
        }
        
        private void RenderMethodIndexChanged(object sender, EventArgs e)
        {
            RenderMethodSelectedIndex = Plot.RenderMethodComboBox.SelectedIndex;
        }
    }
}
