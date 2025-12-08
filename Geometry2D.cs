using System;

namespace kaggle.santa2025
{
    public static class Geometry2D
    {
        /// <summary>
        /// Rotates point X around center Z by exactly 'degrees'.
        /// Special-cased for 180° to preserve perfect precision.
        /// </summary>
        public static (double X, double Y) RotateAroundPoint(
            double centerX, double centerY,
            double pointX, double pointY,
            double degrees)
        {
            // Normalize angle to [-360, +360] and handle common multiples of 90°
            degrees = degrees % 360.0;
            if (degrees < 0) degrees += 360.0;

            // ---------- SPECIAL CASE: exactly 180° (or -180°) ----------
            // This gives mathematically perfect results: no trig functions → no rounding error
            if (Math.Abs(degrees - 180.0) < 1e-9)   // 180° ± tiny epsilon
            {
                double newX = centerX + (centerX - pointX);   // = 2*centerX - pointX
                double newY = centerY + (centerY - pointY);   // = 2*centerY - pointY
                return (newX, newY);
            }

            // Optional: you can also special-case 0°, 90°, 270° for perfect integer results
            if (Math.Abs(degrees) < 1e-9 || Math.Abs(degrees - 360.0) < 1e-9)
                return (pointX, pointY);                                   // 0° → unchanged

            double tx, ty;

            if (Math.Abs(degrees - 90.0) < 1e-9)
            {
                tx = pointX - centerX;
                ty = pointY - centerY;
                return (centerX - ty, centerY + tx);                       // 90° CCW
            }

            if (Math.Abs(degrees - 270.0) < 1e-9)
            {
                tx = pointX - centerX;
                ty = pointY - centerY;
                return (centerX + ty, centerY - tx);                       // 270° CCW = -90°
            }

            // ---------- GENERAL CASE: use trigonometry ----------
            double radians = degrees * Math.PI / 180.0;
            double cos = Math.Cos(radians);
            double sin = Math.Sin(radians);

            tx = pointX - centerX;
            ty = pointY - centerY;

            double rotatedX = tx * cos - ty * sin;
            double rotatedY = tx * sin + ty * cos;
            return (rotatedX + centerX, rotatedY + centerY);
        }
    }
}
