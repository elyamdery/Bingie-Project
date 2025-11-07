using System;
using System.Collections.Generic;
using Bingie.Models;
using Microsoft.Maui.Graphics;

namespace Bingie.Views.Components;

public sealed class TriggerRadarDrawable : IDrawable
{
    public IReadOnlyList<TriggerRadarBucket> Buckets { get; set; } = Array.Empty<TriggerRadarBucket>();

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        canvas.SaveState();
        canvas.FillColor = Colors.Transparent;
        canvas.StrokeColor = Color.FromArgb("#E0E5FF");
        canvas.StrokeSize = 1;

        var center = new PointF(dirtyRect.Width / 2f, dirtyRect.Height / 2f);
        var radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2f - 10;
        var count = Math.Max(Buckets.Count, 3);

        // Draw guidelines
        for (var r = 0.2f; r <= 1f; r += 0.2f)
        {
            canvas.DrawCircle(center, radius * r);
        }

        if (Buckets.Count == 0)
        {
            canvas.RestoreState();
            return;
        }

        // Draw spokes and labels
        for (var i = 0; i < count; i++)
        {
            var angle = (float)(i * (2 * Math.PI / count) - Math.PI / 2);
            var end = new PointF(
                center.X + radius * (float)Math.Cos(angle),
                center.Y + radius * (float)Math.Sin(angle));
            canvas.DrawLine(center, end);

            if (i < Buckets.Count)
            {
                var labelPoint = new PointF(
                    center.X + (radius + 12) * (float)Math.Cos(angle),
                    center.Y + (radius + 12) * (float)Math.Sin(angle));
                canvas.FontSize = 12;
                canvas.FontColor = Colors.White;
                canvas.DrawString(Buckets[i].Label, labelPoint.X, labelPoint.Y, HorizontalAlignment.Center);
            }
        }

        // Draw data polygon
        PathF path = new();
        for (var i = 0; i < Buckets.Count; i++)
        {
            var angle = i * (2 * Math.PI / Buckets.Count) - Math.PI / 2;
            var magnitude = (float)Math.Clamp(Buckets[i].Intensity, 0, 1);
            var point = new PointF(
                center.X + radius * magnitude * (float)Math.Cos(angle),
                center.Y + radius * magnitude * (float)Math.Sin(angle));
            if (i == 0)
            {
                path.MoveTo(point);
            }
            else
            {
                path.LineTo(point);
            }
        }
        path.Close();

        canvas.FillColor = Color.FromRgba(110, 123, 255, 80);
        canvas.FillPath(path);
        canvas.StrokeColor = Color.FromArgb("#6E7BFF");
        canvas.StrokeSize = 2;
        canvas.DrawPath(path);

        canvas.RestoreState();
    }
}
