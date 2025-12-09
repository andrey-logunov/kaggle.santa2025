using System;

namespace kaggle.santa2025
{
    public static class Geometry2D
    {
        static readonly decimal PI = 3.14159265358979323846264338327950288419716939937510m;

        /// <summary>
        /// Rotates point X around center Z by exactly 'degrees'.
        /// Special-cased for 180° to preserve perfect precision.
        /// </summary>
        public static (decimal X, decimal Y) RotateAroundPoint(
            decimal centerX, decimal centerY,
            decimal pointX, decimal pointY,
            decimal degrees)
        {
            // Normalize angle to [-360, +360] and handle common multiples of 90°
            degrees = degrees % 360.0m;
            if (degrees < 0) degrees += 360.0m;

            // ---------- SPECIAL CASE: exactly 180° (or -180°) ----------
            // This gives mathematically perfect results: no trig functions → no rounding error
            if (Math.Abs(degrees - 180.0m) < 1e-9m)   // 180° ± tiny epsilon
            {
                decimal newX = centerX + (centerX - pointX);   // = 2*centerX - pointX
                decimal newY = centerY + (centerY - pointY);   // = 2*centerY - pointY
                return (newX, newY);
            }

            // Optional: you can also special-case 0°, 90°, 270° for perfect integer results
            if (Math.Abs(degrees) < 1e-9m || Math.Abs(degrees - 360.0m) < 1e-9m)
                return (pointX, pointY);                                   // 0° → unchanged

            decimal tx, ty;

            if (Math.Abs(degrees - 90.0m) < 1e-9m)
            {
                tx = pointX - centerX;
                ty = pointY - centerY;
                return (centerX - ty, centerY + tx);                       // 90° CCW
            }

            if (Math.Abs(degrees - 270.0m) < 1e-9m)
            {
                tx = pointX - centerX;
                ty = pointY - centerY;
                return (centerX + ty, centerY - tx);                       // 270° CCW = -90°
            }

            // ---------- GENERAL CASE: use trigonometry ----------
            decimal radians = degrees * PI / 180.0m;
            decimal cos = (decimal)Math.Cos((double)radians);
            decimal sin = (decimal)Math.Sin((double)radians);

            tx = pointX - centerX;
            ty = pointY - centerY;

            decimal rotatedX = tx * cos - ty * sin;
            decimal rotatedY = tx * sin + ty * cos;
            return (rotatedX + centerX, rotatedY + centerY);
        }
    }
}
