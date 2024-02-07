using UnityEngine;

namespace GameCore.CrossScene.Scripts.Calculations
{
    public static class MatrixExtension
    {
        public static int[,] RotateMatrix(this int[,] matrix, int angle)
        {
            int[,] newMatrix = (int[,])matrix.Clone();
            int rotations = angle / 90;
            switch (rotations)
            {
                case 1:
                    return RotateMatrix90(newMatrix);
                case 2:
                    return MirrorVertical(MirrorHorizontal(newMatrix));
                case 3:
                    return MirrorHorizontal(MirrorVertical(RotateMatrix90(matrix)));
                default:
                    return newMatrix;
            }
        }

        public static int[,] MirrorHorizontal(int[,] matrix)
        {
            int[,] newMatrix = (int[,])matrix.Clone();
            int rows = newMatrix.GetLength(0);
            int cols = newMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                newMatrix[i, j] = matrix[i, cols - 1 - j];

            return newMatrix;
        }
        
        public static int[,] MirrorVertical(int[,] matrix)
        {
            int[,] newMatrix = (int[,])matrix.Clone();
            int rows = newMatrix.GetLength(0);
            int cols = newMatrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                newMatrix[i, j] = matrix[rows - 1 - i, j];

            return newMatrix;
        }
        
        private static int[,] RotateMatrix90(int[,] matrix, int coefficient = 1)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            int[,] rotatedMatrix = new int[cols, rows];

            
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (coefficient == 1)
                        rotatedMatrix[j, rows - 1 - i] = matrix[i, j];
                    else
                        rotatedMatrix[cols - 1 - j, i] = matrix[i, j];
                }
            }

            return rotatedMatrix;
        }
    }
}