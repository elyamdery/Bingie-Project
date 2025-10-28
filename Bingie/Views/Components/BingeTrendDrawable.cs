using System;
using System.Collections.Generic;
using System.Linq;
using Bingie.Models;
using Microsoft.Maui.Graphics;

namespace Bingie.Views.Components;

public class BingeTrendDrawable : IDrawable
{
    private IReadOnlyList<BingeDayStat> _data = Array.Empty<BingeDayStat>();

    public void UpdateData(IReadOnlyList<BingeDayStat> data)
    {
        _data = data ?? Array.Empty<BingeDayStat>();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.FillColor = Colors.Transparent;
        canvas.FillRectangle(dirtyRect);

        if (_data.Count == 0)
        {
            canvas.FontColor = Colors.White.WithAlpha(0.6f);
            canvas.FontSize = 16;
            canvas.DrawString("No binge data yet — log a moment to see your trend.", dirtyRect,
                HorizontalAlignment.Center, VerticalAlignment.Center);
            canvas.RestoreState();
            return;
        }

        const float margin = 28f;
        var plotWidth = dirtyRect.Width - (margin * 2);
        var plotHeight = dirtyRect.Height - (margin * 2);
        if (plotWidth <= 0 || plotHeight <= 0)
        {
            canvas.RestoreState();
            return;
        }

        var maxValue = Math.Max(1f,
            (float)Math.Max(_data.Max(d => d.Count), _data.Max(d => d.MovingAverage)));

        void DrawAxisLines()
        {
            canvas.StrokeColor = Colors.White.WithAlpha(0.2f);
            canvas.StrokeSize = 1;
            canvas.DrawLine(margin, dirtyRect.Height - margin, dirtyRect.Width - margin,
                dirtyRect.Height - margin);
            canvas.DrawLine(margin, margin, margin, dirtyRect.Height - margin);

            var stepValue = Math.Max(1, (int)Math.Ceiling(maxValue / 4f));
            for (var i = stepValue; i <= maxValue; i += stepValue)
            {
                var y = dirtyRect.Height - margin - (i / maxValue) * plotHeight;
                canvas.DrawLine(margin, y, dirtyRect.Width - margin, y);
                canvas.FontColor = Colors.White.WithAlpha(0.4f);
                canvas.FontSize = 12;
                canvas.DrawString(i.ToString(), 4, y - 8, HorizontalAlignment.Left);
            }
        }

        DrawAxisLines();

        void DrawSeries(Func<BingeDayStat, float> selector, Color color, float strokeWidth, bool fillPoints)
        {
            canvas.StrokeColor = color;
            canvas.StrokeSize = strokeWidth;
            var path = new PathF();

            for (var index = 0; index < _data.Count; index++)
            {
                var stat = _data[index];
                var x = margin + (plotWidth / Math.Max(1, _data.Count - 1) * index);
                var y = dirtyRect.Height - margin - (selector(stat) / maxValue) * plotHeight;

                if (index == 0)
                {
                    path.MoveTo(x, y);
                }
                else
                {
                    path.LineTo(x, y);
                }

                if (!fillPoints) continue;
                canvas.FillColor = color;
                canvas.FillCircle(x, y, 4);
            }

            canvas.DrawPath(path);
        }

        DrawSeries(stat => stat.Count, Color.FromArgb("#FF6B6B"), 3, true);
        DrawSeries(stat => (float)stat.MovingAverage, Color.FromArgb("#FFE66D"), 2, false);

        canvas.RestoreState();
    }
}
