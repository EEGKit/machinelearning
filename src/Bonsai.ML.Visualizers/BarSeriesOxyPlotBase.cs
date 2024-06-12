﻿using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Drawing;
using System;
using OxyPlot.Axes;
using System.Collections;

namespace Bonsai.ML.Visualizers
{
    internal class BarSeriesOxyPlotBase : UserControl
    {
        private PlotView view;
        private PlotModel model;
        private OxyColor defaultBarSeriesColor = OxyColors.LightBlue;

        private Axis xAxis;
        private Axis yAxis;

        private StatusStrip statusStrip;

        /// <summary>
        /// Gets or sets the integer value that determines how many data points should be shown along the x axis.
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Gets the status strip control.
        /// </summary>
        public StatusStrip StatusStrip => statusStrip;

        /// <summary>
        /// Initializes a new instance of the <see cref="BarSeriesOxyPlotBase"/> class
        /// </summary>
        public BarSeriesOxyPlotBase()
        {
            Initialize();
        }

        private void Initialize()
        {
            view = new PlotView
            {
                Size = Size,
                Dock = DockStyle.Fill,
            };

            model = new PlotModel();

            xAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Category",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                FontSize = 9,
                Key = "y1"
            };

            yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Value",
                Key = "x1"
            };

            model.Axes.Add(xAxis);
            model.Axes.Add(yAxis);

            view.Model = model;
            Controls.Add(view);

            statusStrip = new StatusStrip
            {
                Visible = false
            };

            view.MouseClick += new MouseEventHandler(onMouseClick);
            Controls.Add(statusStrip);

            Controls.Add(statusStrip);

            AutoScaleDimensions = new SizeF(6F, 13F);
        }

        private void onMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                statusStrip.Visible = !statusStrip.Visible;
            }
        }

        /// <summary>
        /// Method to add a new bar series to the data plot.
        /// Requires a string for the name of the bar series
        /// Fill color of the bar series is optional.
        /// </summary>
        public BarSeries AddNewBarSeries(string barSeriesName, OxyColor? fillColor = null)
        {
            OxyColor _fillColor = fillColor.HasValue ? fillColor.Value : defaultBarSeriesColor;
            BarSeries barSeries = new BarSeries
            {
                Title = barSeriesName,
                FillColor = _fillColor,
                StrokeColor = OxyColors.Black,
                XAxisKey = "x1",
                YAxisKey = "y1"
            };
            AddSeriesToModel(barSeries);
            return barSeries;
        }

        /// <summary>
        /// Method to add a new bar series to the data plot.
        /// Requires a string for the name of the bar series
        /// Fill color of the bar series is optional.
        /// </summary>
        public ErrorBarSeries AddNewErrorBarSeries(string barSeriesName, OxyColor? fillColor = null)
        {
            OxyColor _fillColor = fillColor.HasValue ? fillColor.Value : defaultBarSeriesColor;
            ErrorBarSeries errorBarSeries = new ErrorBarSeries
            {
                Title = barSeriesName,
                FillColor = _fillColor,
                StrokeColor = OxyColors.Black,
                XAxisKey = "x1",
                YAxisKey = "y1"
            };
            AddSeriesToModel(errorBarSeries);
            return errorBarSeries;
        }

        /// <summary>
        /// Method to add a series to the plot model.
        /// Requires the series.
        /// </summary>
        public void AddSeriesToModel(Series series)
        {
            model.Series.Add(series);
        }

        /// <summary>
        /// Method to add a value to a bar series.
        /// Requires the bar series and value.
        /// </summary>
        public void AddValueToBarSeries(BarSeries barSeries, double value)
        {
            var barItem = new BarItem { Value = value };
            AddBarItemToBarSeries(barSeries, barItem);
        }

        /// <summary>
        /// Method to add value with error to a bar series.
        /// Requires the bar series, value, and error.
        /// </summary>
        public void AddValueAndErrorToBarSeries(BarSeries barSeries, double value, double error)
        {
            var barItem = new ErrorBarItem { Value = value, Error = error };
            AddBarItemToBarSeries(barSeries, barItem);
        }

        /// <summary>
        /// Method to add bar item to a bar series.
        /// Requires the bar item and bar series.
        /// </summary>
        public void AddBarItemToBarSeries(BarSeries barSeries, BarItem barItem)
        {
            barSeries.Items.Add(barItem);
        }

        /// <summary>
        /// Set the minimum and maximum values to show along the y axis.
        /// Requires the minValue and maxValue.
        /// </summary>
        public void SetAxes(double minValue, double maxValue)
        {
            yAxis.Minimum = minValue;
            yAxis.Maximum = maxValue;
        }

        /// <summary>
        /// Method to update the plot.
        /// </summary>
        public void UpdatePlot()
        {
            model.InvalidatePlot(true);
        }

        /// <summary>
        /// Method to reset the bar series.
        /// </summary>
        public void ResetBarSeries(BarSeries barSeries)
        {
            barSeries.Items.Clear();
        }

        /// <summary>
        /// Method to reset all series in the current PlotModel.
        /// </summary>
        public void ResetModelSeries()
        {
            model.Series.Clear();
        }

        /// <summary>
        /// Method to reset the x and y axes to their default.
        /// </summary>
        public void ResetAxes()
        {
            xAxis.Reset();
            yAxis.Reset();
        }
    }
}