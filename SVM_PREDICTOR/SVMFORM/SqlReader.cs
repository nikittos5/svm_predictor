using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace SVMFORM
{
    class SqlReader
    {
        private string currentTable;
        private string connectionString;
        private SqlConnection sqlConnection;
        private int skips = 493;
        private int startId = 1023;
        private int tempId = 1023;
        public SqlReader(string connectionString, string dataTableName)
        {
            sqlConnection = new SqlConnection(connectionString);
            this.connectionString = connectionString;
            currentTable = dataTableName;
        }
        public Data GetNextData()
        {
            sqlConnection.Open();
            SqlDataReader sqlDataReader = new SqlCommand("Select * FROM "+currentTable+" WHERE id="+(tempId++).ToString(), sqlConnection).ExecuteReader();
            if (sqlDataReader.Read())
            {
                int shift = 0;
                HashSet<int> zeros = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41,
                42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 79, 80, 81, 82, 83, 85, 87, 88, 89, 94, 95, 96, 97, 98, 100,
                102, 103, 104, 109, 110, 111, 112, 113, 115, 117, 118, 119, 124, 125, 126, 127, 128, 129, 130, 132, 133, 134, 135, 139, 140, 141, 142, 143, 144, 145, 147, 148, 149, 150, 154, 155, 156, 157, 158, 160,
                162, 163, 164, 169, 170, 171, 172, 173, 175, 177, 178, 179, 184, 185, 186, 187, 188, 190, 191, 192, 193, 194, 199, 200, 201, 202, 203, 205, 207, 208, 209, 214, 215, 216, 217, 218, 220, 222, 223, 224,
                229, 230, 231, 232, 233, 235, 237, 238, 239, 244, 245, 246, 247, 248, 250, 252, 253, 254, 259, 260, 261, 262, 263, 265, 267, 268, 269, 274, 275, 276, 277, 278, 280, 282, 283, 284, 289, 290, 291, 292,
                293, 294, 295, 296, 297, 298, 299, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 319, 320, 321, 322, 323, 324, 325, 326, 327, 328, 329, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344,
                346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 377, 379, 380, 381, 382, 383, 384, 385, 386, 387,
                388, 389, 390, 394, 395, 396, 397, 398, 399, 400, 402, 403, 404, 405, 409, 410, 411, 412, 413, 415, 417, 418, 419, 424, 425, 426, 427, 428, 430, 432, 433, 434, 436, 437, 438, 439, 440, 441, 442, 443,
                444, 445, 446, 447, 448, 449, 450, 454, 455, 456, 457, 458, 460, 462, 463, 464, 469, 470, 471, 472, 473, 475, 477, 478, 479, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495,
                496, 497, 498, 499, 501, 502, 505, 507, 508, 509, 512, 513, 514, 515, 516, 517, 518, 519, 520, 521, 522, 523, 524, 525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538, 539, 540, 541,
                542, 543, 544, 545, 546, 547, 548, 549, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578, 579, 580, 581, 582,
                583, 584, 585, 586, 587, 588, 589, 590, 591, 592, 593, 594, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 615, 616, 617, 618, 619, 620, 621, 622,
                623, 624 };
                Data data = new Data(sqlDataReader.FieldCount - skips);
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    if (zeros.Contains(i))
                    {
                        shift++;
                        continue;
                    }
                    if (i<512)
                        data[i - shift] = sqlDataReader.GetFloat(i);
                    else
                        data[i - shift] = Convert.ToDouble(sqlDataReader.GetBoolean(i));
                }
                sqlDataReader.Close();
                sqlConnection.Close();
                return data;
            }
            else
            {
                sqlDataReader.Close();
                sqlConnection.Close();
                return new Data(sqlDataReader.FieldCount - skips);
            }
        }
        public DataTable GetLastNData(int N)
        {
            sqlConnection.Open();
            int shift;
            HashSet<int> zeros = new HashSet<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 
                42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 79, 80, 81, 82, 83, 85, 87, 88, 89, 94, 95, 96, 97, 98, 100, 
                102, 103, 104, 109, 110, 111, 112, 113, 115, 117, 118, 119, 124, 125, 126, 127, 128, 129, 130, 132, 133, 134, 135, 139, 140, 141, 142, 143, 144, 145, 147, 148, 149, 150, 154, 155, 156, 157, 158, 160,
                162, 163, 164, 169, 170, 171, 172, 173, 175, 177, 178, 179, 184, 185, 186, 187, 188, 190, 191, 192, 193, 194, 199, 200, 201, 202, 203, 205, 207, 208, 209, 214, 215, 216, 217, 218, 220, 222, 223, 224,
                229, 230, 231, 232, 233, 235, 237, 238, 239, 244, 245, 246, 247, 248, 250, 252, 253, 254, 259, 260, 261, 262, 263, 265, 267, 268, 269, 274, 275, 276, 277, 278, 280, 282, 283, 284, 289, 290, 291, 292,
                293, 294, 295, 296, 297, 298, 299, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 314, 319, 320, 321, 322, 323, 324, 325, 326, 327, 328, 329, 334, 335, 336, 337, 338, 339, 340, 341, 342, 343, 344,
                346, 347, 348, 349, 350, 351, 352, 353, 354, 355, 356, 357, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 368, 369, 370, 371, 372, 373, 374, 375, 377, 379, 380, 381, 382, 383, 384, 385, 386, 387, 
                388, 389, 390, 394, 395, 396, 397, 398, 399, 400, 402, 403, 404, 405, 409, 410, 411, 412, 413, 415, 417, 418, 419, 424, 425, 426, 427, 428, 430, 432, 433, 434, 436, 437, 438, 439, 440, 441, 442, 443, 
                444, 445, 446, 447, 448, 449, 450, 454, 455, 456, 457, 458, 460, 462, 463, 464, 469, 470, 471, 472, 473, 475, 477, 478, 479, 481, 482, 483, 484, 485, 486, 487, 488, 489, 490, 491, 492, 493, 494, 495, 
                496, 497, 498, 499, 501, 502, 505, 507, 508, 509, 512, 513, 514, 515, 516, 517, 518, 519, 520, 521, 522, 523, 524, 525, 526, 527, 528, 529, 530, 531, 532, 533, 534, 535, 536, 537, 538, 539, 540, 541, 
                542, 543, 544, 545, 546, 547, 548, 549, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578, 579, 580, 581, 582, 
                583, 584, 585, 586, 587, 588, 589, 590, 591, 592, 593, 594, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 610, 611, 612, 613, 614, 615, 616, 617, 618, 619, 620, 621, 622, 
                623, 624 };
            SqlDataReader sqlDataReader = new SqlCommand("Select COUNT(*) from " + currentTable, sqlConnection).ExecuteReader();
            sqlDataReader.Read();
            int rowsCount = sqlDataReader.GetInt32(0);
            sqlDataReader.Close();
            sqlDataReader = new SqlCommand("Select * FROM " + currentTable + " WHERE id>=" + (startId+rowsCount-N).ToString(), sqlConnection).ExecuteReader();
            double[][] dataTable = new double[N][];
            DataTable dTable = new DataTable();
            for (int i = 0; i< N; i++)
            {
                sqlDataReader.Read();
                dataTable[i] = new double[sqlDataReader.FieldCount-skips];
                shift = 0;
                for (int j = 0; j < sqlDataReader.FieldCount; j++)
                {
                    if (zeros.Contains(j))
                    {
                        shift++;
                        continue;
                    }
                    else
                    if (j < 512)
                    {
                        dataTable[i][j - shift] = sqlDataReader.GetFloat(j);
                    }
                    else
                        dataTable[i][j - shift] = Convert.ToDouble(sqlDataReader.GetBoolean(j));
                }
            }
            for (int j = 0; j < sqlDataReader.FieldCount - skips; j++)
                dTable.Columns.Add("Column"+j.ToString(), typeof(double));
            for(int i = 0; i<N; i++)
            {
                var row = dTable.NewRow();
                for (int j = 0; j < sqlDataReader.FieldCount - skips; j++)
                {
                    row[j] = dataTable[i][j];
                }
                dTable.Rows.Add(row);
            }
            sqlDataReader.Close();
            sqlConnection.Close();
            return dTable;
        }
    }
}
