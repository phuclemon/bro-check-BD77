using System;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

class Program
{
    // Danh sách chứa dữ liệu từ file CSV
    static List<List<string>> columnDataList = new List<List<string>>();

    // Đường dẫn đến file CSV
    static string csvFilePath = @"Book1.csv";

    // Đổi màu nhanh 
    static void White()
    {
        Console.ForegroundColor = ConsoleColor.White;
    }

    static void DCyan()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
    }

    static void Yellow()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
    }
    // Kiểm tra số nguyên dương
    static bool IsPositiveInteger(string input)
    {
        if (double.TryParse(input, out double result))
        {
            return result > 0;
        }

        return false;
    }
    static void FileComposer()
    {
        try
        {
            using (StreamReader reader = new StreamReader(csvFilePath))
            {
                string line;
                // Đọc từng dòng trong file CSV
                while ((line = reader.ReadLine()) != null)
                {
                    // Tách các cột trong dòng bằng dấu phẩy
                    string[] columns = line.Split(',');

                    // Duyệt qua từng cột
                    for (int i = 0; i < columns.Length; i++)
                    {
                        // Kiểm tra và thêm dữ liệu vào danh sách cột
                        if (i >= columnDataList.Count)
                        {
                            columnDataList.Add(new List<string>());
                        }

                        // Loại bỏ khoảng trắng không mong muốn và thêm vào danh sách
                        string cleanedValue = columns[i].Trim();
                        columnDataList[i].Add(cleanedValue);
                    }
                }
            }
            // Gọi hàm hiển thị bảng sau khi đọc dữ liệu
        }
        catch (IOException e)
        {
            Console.WriteLine("Lỗi khi đọc file: " + e.Message);
        }
    }

    // Hàm hiển thị bảng dữ liệu
    static void TableDisplay()
    {
        // Tính toán độ rộng tối đa của mỗi cột
        List<int> columnWidths = new List<int>();
        for (int i = 0; i < columnDataList.Count; i++)
        {
            int maxWidth = columnDataList[i][0].Length;
            for (int j = 0; j < columnDataList[i].Count; j++)
            {
                int length = columnDataList[i][j].Replace("\"", "").Length;
                if (length > maxWidth)
                {
                    maxWidth = length;
                }
            }

            columnWidths.Add(maxWidth);
        }

        // In header
        for (int i = 0; i < columnDataList.Count; i++)
        {
            string header = columnDataList[i][0].Trim('\"');
            Console.Write($"| {header} ".PadRight(columnWidths[i] + 3));
        }

        Console.WriteLine("|");


        // In đường kẻ ngăn cách header và dữ liệu
        for (int i = 0; i < columnDataList.Count; i++)
        {
            Console.Write($"+{new string('-', columnWidths[i] + 2)}");
        }

        Console.WriteLine("+");

        // In dữ liệu
        for (int i = 1; i < GetMaxLength(); i++)
        {
            for (int j = 0; j < columnDataList.Count; j++)
            {
                if (columnDataList[j].Count > i)
                {
                    string value = columnDataList[j][i].Replace("\"", "");
                    Console.Write($"| {value.PadRight(columnWidths[j])} ");
                }
                else
                {
                    Console.Write($"| {"".PadRight(columnWidths[j])} ");
                }
            }

            Console.WriteLine("|");
        }
    }
    static void DrawProfitHistogram()
    {
        // Lấy danh sách tên sản phẩm và lợi nhuận tương ứng
        List<string> productNames = new List<string>();
        List<double> profits = new List<double>();

        for (int i = 1; i < GetMaxLength(); i++)
        {
            string productName = columnDataList[1][i].Replace("\"", ""); // Lấy tên sản phẩm từ cột "Tên sản phẩm"
            if (double.TryParse(columnDataList[6][i], out double revenue)) // Lấy doanh thu từ cột "Lợi nhuận"
            {
                productNames.Add(productName);
                profits.Add(revenue);
            }
        }

        // Tìm giá trị lớn nhất trong danh sách lợi nhuận
        double MaxProfits = profits.Max();

        // Vẽ biểu đồ
        Yellow();
        Console.WriteLine("        -----Biểu đồ lợi nhuận----- ");
        DCyan();
        for (int i = 0; i < productNames.Count; i++)
        {
            White();
            Console.Write(productNames[i].PadRight(10) + " | ");
            int barLength = (int)(profits[i] / MaxProfits * 50); // Chiều dài của thanh biểu đồ
            DCyan();
            for (int j = 0; j < barLength; j++)
            {
                Console.Write("*");
            }

            White();
            Console.WriteLine($" ({profits[i]})");
        }
    }
    static void DrawSalesHistogram()
    {
        // Lấy danh sách tên sản phẩm và số lượng bán ra tương ứng
        List<string> productNames = new List<string>();
        List<int> salesQuantities = new List<int>();

        for (int i = 1; i < GetMaxLength(); i++)
        {
            string productName = columnDataList[1][i].Replace("\"", ""); // Lấy tên sản phẩm từ cột "Tên sản phẩm"
            if (int.TryParse(columnDataList[5][i], out int salesQuantity)) // Lấy số lượng bán ra từ cột "Số lượng bán ra"
            {
                productNames.Add(productName);
                salesQuantities.Add(salesQuantity);
            }
        }

        // Tìm giá trị lớn nhất trong danh sách số lượng bán ra
        int maxSalesQuantity = salesQuantities.Max();

        // Vẽ biểu đồ
        Yellow();
        Console.WriteLine("        -----Biểu đồ số lượng bán ra-----");
        for (int i = 0; i < productNames.Count; i++)
        {
            White();
            Console.Write(productNames[i].PadRight(10) + " | ");
            DCyan();
            int barLength = (int)(salesQuantities[i] / (double)maxSalesQuantity * 50); // Chiều dài của thanh biểu đồ
            for (int j = 0; j < barLength; j++)
            {
                Console.Write("*");
            }

            White();
            Console.WriteLine($" ({salesQuantities[i]})");
        }
    }
    // Hàm trả về số lượng hàng tối đa trong danh sách cột
    static int GetMaxLength()
    {
        int maxLength = 0;
        foreach (var arr in columnDataList)
        {
            if (arr is List<string> list)
            {
                int length = list.Count;
                if (length > maxLength)
                {
                    maxLength = length;
                }
            }
        }

        return maxLength;
    }

    //  Hàm tìm kiếm thông tin hàng
    static void FindInformation()
{
    // Khởi tạo map đưa cột 0 vào ( cột mã)
    List<string> maspList = new List<string>();
    string masp;
    for (int columnNumber = 0; columnNumber < columnDataList[0].Count; columnNumber++)
    {
        masp = columnDataList[0][columnNumber];
        maspList.Add(masp);
    }

    Yellow();
    Console.Write("Nhập mã hàng muốn truy xuất thông tin: ");
    White();
    string checkCodecheck = Console.ReadLine();
    string checkCode = checkCodecheck.Replace(",", "-");

    if (maspList.Contains(checkCode))
    {
        int rowIndex = maspList.IndexOf(checkCode);

        // Tính toán độ rộng tối đa của mỗi cột
        List<int> columnWidths = new List<int>();
        for (int i = 0; i < columnDataList.Count; i++)
        {
            int maxWidth = columnDataList[i][0].Length;
            for (int j = 0; j < columnDataList[i].Count; j++)
            {
                int length = columnDataList[i][j].Replace("\"", "").Length;
                if (length > maxWidth)
                {
                    maxWidth = length;
                }
            }

            columnWidths.Add(maxWidth);
        }

        Yellow();
        // In header
        for (int i = 0; i < columnDataList.Count; i++)
        {
            string header = columnDataList[i][0].Trim('\"');
            Console.Write($"| {header} ".PadRight(columnWidths[i] + 3));
        }
        Console.WriteLine("|");

        // In đường kẻ ngăn cách header và dữ liệu
        for (int i = 0; i < columnDataList.Count; i++)
        {
            Console.Write($"+{new string('-', columnWidths[i] + 2)}");
        }
        Console.WriteLine("+");

        // In dữ liệu cho mã hàng cụ thể
        for (int i = rowIndex; i < rowIndex + 1; i++)
        {
            for (int j = 0; j < columnDataList.Count; j++)
            {
                string value = columnDataList[j][i].Replace("\"", "");
                Console.Write($"| {value.PadRight(columnWidths[j])} ");
            }
            Console.WriteLine("|");
        }
    }
    else
    {
        Yellow();
        Console.WriteLine("Không tìm thấy mã hàng.");
    }
}

    

    // Hàm thêm một hàng mới vào danh sách cột

    static void AddRow()
    {
        Yellow();
        Console.WriteLine("Nhập dữ liệu cho hàng mới:");

        double giaVon = 0;
        double giaBan = 0;
        double slban = 0;
        double slnhap = 0;
        for (int i = 0; i < columnDataList.Count; i++)
        {
            if (i == 0)
            {
                White();
                Console.Write($"Nhập giá trị cho ");
                DCyan();
                Console.Write($"{columnDataList[i][0].Replace("\"", "")}");
                White();
                Console.Write(": ");

                White();
                string userInputcheck = Console.ReadLine();
                string userInput = userInputcheck.Replace(",", "-");
                if (userInput == "") userInput = "null";

                if (columnDataList[i].Contains(userInput.ToUpper()))
                {
                    Yellow();
                    Console.WriteLine(" Mã sản phẩm đã tồn tại, vui lòng kiểm tra lại! ");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    columnDataList[i].Add(userInput.ToUpper());
                }
            }

            else if (i == 2 || i == 3)
            {
                Yellow();
                White();
                Console.Write("Nhập giá trị cho ");
                DCyan();
                Console.Write($"{columnDataList[i][0].Replace("\"", "")}");
                White();
                Console.Write(": ");
                Console.Write("Nhập số nguyên dương: ");
                White();
                string userInput = Console.ReadLine();

                if (IsPositiveInteger(userInput))
                {
                    if (i == 2)
                    {
                        slnhap = double.Parse(userInput);
                    }
                    else if (i == 3)
                    {
                        giaVon = double.Parse(userInput);
                    }

                    columnDataList[i].Add(userInput);
                }
                else
                {
                    Yellow();
                    Console.WriteLine("Giá trị không hợp lệ. Số nguyên dương được yêu cầu. Vui lòng nhập lại.");
                    i--; // Quay lại cột hiện tại
                }
            }
            else if (i == 4)
            {
                while (true)
                {
                    Yellow();
                    Console.Write("Nhập giá trị cho Giá Bán (phải lớn hơn hoặc bằng giá nhập): ");
                    White();
                    string giaBanInput = Console.ReadLine();

                    if (IsPositiveInteger(giaBanInput) && double.Parse(giaBanInput) >= giaVon)
                    {
                        giaBan = double.Parse(giaBanInput);
                        columnDataList[i].Add(giaBanInput);

                        break;
                    }
                }
            }
            else if (i == 5)
            {
                while (true)
                {
                    Yellow();
                    Console.Write("Nhập số lượng Bán (phải bé hơn hoặc bằng số lượng nhập): ");
                    White();
                    string slBanInput = Console.ReadLine();

                    if (IsPositiveInteger(slBanInput) && double.Parse(slBanInput) <= slnhap)
                    {
                        slban = double.Parse(slBanInput);
                        columnDataList[i].Add(slBanInput);

                        break;
                    }
                }
            }
            else if (i == 1 || i == 7)
            {
                White();
                Console.Write($"Nhập ");
                DCyan();
                Console.Write($"{columnDataList[i][0].Replace("\"", "")}");
                White();
                Console.Write(": (Nhập chuỗi) : ");

                White();
                string userInputcheck = Console.ReadLine();
                string userInput = userInputcheck.Replace(",", "-");
                if (userInput == "") userInput = "null";
                columnDataList[i].Add(userInput);
            }
        }

        // Tính lợi nhuận và thêm vào danh sách
        double loiNhuan = (giaBan - giaVon) * (slban);
        columnDataList[columnDataList.Count - 2].Add(loiNhuan.ToString());
        SaveToCsv();
    }

    // Hàm đọc dữ liệu từ file CSV và tạo danh sách cột

    // Hàm chỉnh sửa giá trị ô trong bảng -- cần sửa
    static void EditCellValue()
    {
        // Đã fix phần tra cứu theo mã sản phẩm.
        List<string> maspList = new List<string>();
        string masp;
        for (int columnNumber = 0; columnNumber < columnDataList[0].Count; columnNumber++)
        {
            masp = columnDataList[0][columnNumber];
            maspList.Add(masp);
        }

        while (true)
        {
            Yellow();
            Console.Write("Nhập mã sản phẩm bạn muốn chỉnh sửa, nhấn [Enter] để dừng chức năng: ");
            White();
            string checkCode = Console.ReadLine();
            if (checkCode.ToLower() == "")
            {
                break;
            }

            if (maspList.Contains(checkCode))
            {
                int rowIndex = maspList.IndexOf(checkCode);
                if (rowIndex >= 1 && rowIndex <= GetMaxLength())
                {
                    Yellow();
                    Console.Write("Nhập 0 để sửa toàn bộ thông tin sản phẩm hoặc nhập 1 để sửa một thông tin nhất định: ");
                    White();
                    if (int.TryParse(Console.ReadLine(), out int check) &&
                        (check == 0 || check == 1)) // có sửa qua chỗ này + 1 tryparse
                    {
                        if (check == 0)
                        {
                            White();
                            Console.Write($"Bạn đang sửa toàn bộ thông tin của mã ");
                            DCyan();
                            Console.Write($"{checkCode}");
                            White();
                            Console.WriteLine(".");


                            // Thêm code xóa hàng
                            for (int k = 0; k < columnDataList.Count; k++)
                            {
                                columnDataList[k].RemoveAt(rowIndex);
                            }

                            // Nhập lại dữ liệu mới cho hàng vừa xóa.
                            AddRow();
                            White();
                            Console.WriteLine($"Mã hàng ");
                            DCyan();
                            Console.Write($"{checkCode}");
                            White();
                            Console.WriteLine(" đã được chỉnh sửa!");

                            SaveToCsv();
                        }
                        else if (check == 1)
                        {
                            White();
                            Console.WriteLine("1 : Mã sản phẩm");
                            Console.WriteLine("2 : Tên sản phẩm");
                            Console.WriteLine("3 : Số lượng nhập");
                            Console.WriteLine("4 : Giá vốn");
                            Console.WriteLine("5 : Giá bán ra ");
                            Console.WriteLine("6 : Số lượng bán ra");
                            Console.WriteLine("7 : Lợi nhuận");
                            Console.WriteLine("8 : Ghi chú");
                            Yellow();
                            Console.Write("Nhập số cột của thông tin bạn muốn chỉnh sửa: ");
                            White();
                            if (int.TryParse(Console.ReadLine(), out int columnIndex) && columnIndex >= 1 &&
                                columnIndex <= columnDataList.Count)
                            {
                                // Kiểm tra xem cột "Lợi nhuận" có thể chỉnh sửa hay không
                                if (columnIndex == 7)
                                {
                                    int i = rowIndex;
                                    if (float.TryParse(columnDataList[6][i], out float loiNhuanValue) &&
                                        loiNhuanValue != 0 ||
                                        loiNhuanValue.ToString() == string.Empty) // Kiểm tra giá trị Lợi nhuận hiện tại
                                    {
                                        Yellow();
                                        Console.WriteLine("Không thể chỉnh sửa cột Lợi Nhuận");
                                        Console.ReadKey();
                                        return;
                                    }
                                    else
                                    {
                                        // Tính toán giá trị lợi nhuận dựa trên các cột khác
                                        double giaVon, giaBanRa, soLuongBanRa;

                                        if (double.TryParse(columnDataList[3][i], out giaVon) &&
                                            double.TryParse(columnDataList[4][i], out giaBanRa) &&
                                            double.TryParse(columnDataList[5][i], out soLuongBanRa))
                                        {
                                            double loiNhuan = (giaBanRa - giaVon) * soLuongBanRa;
                                            columnDataList[6][i] = loiNhuan.ToString(); // Cập nhật giá trị lợi nhuận\
                                            Console.WriteLine($"Cập nhật giá trị Lợi nhuận thành công!");
                                            SaveToCsv();
                                        }
                                    }
                                }
                                if (columnIndex == 4 || columnIndex == 5 || columnIndex == 6)
                                {
                                    Yellow();
                                    Console.WriteLine(" * Điều này sẽ dẫn đến sự thay đổi về giá trị trong cột Lợi nhuận * ");
                                    Console.Write($"Nhập giá trị mới cho {columnDataList[columnIndex - 1][0]} của hàng {columnDataList[0][rowIndex]}: ");
                                    string newValue = Console.ReadLine();

                                    double giaVon, giaBanRa, soLuongBanRa;
                                    int i = rowIndex;

                                    switch (columnIndex)
                                    {
                                        case 4:
                                            {
                                                Console.WriteLine("* Hãy kiểm tra mức độ hợp lý giữa giá vốn và giá bán trước khi chỉnh sửa *");
                                                if (double.TryParse(columnDataList[4][i], out giaBanRa) &&
                                                    double.TryParse(newValue, out giaVon) &&
                                                    double.TryParse(columnDataList[5][i], out soLuongBanRa))
                                                {
                                                    double loiNhuan = (giaBanRa - giaVon) * soLuongBanRa;
                                                    columnDataList[6][i] = loiNhuan.ToString(); // Cập nhật giá trị lợi nhuận
                                                    SaveToCsv();
                                                    columnDataList[4][i] = giaVon.ToString(); // Cập nhật giá trị giá vốn
                                                    Console.WriteLine(loiNhuan);
                                                    Console.WriteLine($"Cập nhật giá trị Giá vốn và Lợi nhuận thành công!");
                                                    SaveToCsv();
                                                }
                                                break;
                                            }
                                        case 5:
                                            {
                                                Console.WriteLine("* Hãy kiểm tra mức độ hợp lý giữa giá vốn và giá bán trước khi chỉnh sửa *");
                                                if (double.TryParse(columnDataList[3][i], out giaVon) &&
                                                    double.TryParse(newValue, out giaBanRa) &&
                                                    double.TryParse(columnDataList[5][i], out soLuongBanRa))
                                                {
                                                    double loiNhuan = (giaBanRa - giaVon) * soLuongBanRa;
                                                    columnDataList[6][i] = loiNhuan.ToString(); // Cập nhật giá trị lợi nhuận
                                                    SaveToCsv();
                                                    columnDataList[4][i] = giaBanRa.ToString(); // Cập nhật giá trị giá bán ra
                                                    Console.WriteLine(loiNhuan);
                                                    Console.WriteLine($"Cập nhật giá trị Giá bán ra và Lợi nhuận thành công!");
                                                    SaveToCsv();
                                                }
                                                break;
                                            }
                                        case 6:
                                            {
                                                Console.WriteLine("* Hãy kiểm tra mức độ hợp lý giữa số lượng nhập và số lượng bán ra trước khi chỉnh sửa *");
                                                if (double.TryParse(columnDataList[3][i], out giaVon) &&
                                                    double.TryParse(newValue, out soLuongBanRa) &&
                                                    double.TryParse(columnDataList[4][i], out giaBanRa))
                                                {
                                                    double loiNhuan = (giaBanRa - giaVon) * soLuongBanRa;
                                                    columnDataList[6][i] = loiNhuan.ToString(); // Cập nhật giá trị lợi nhuận
                                                    SaveToCsv();
                                                    columnDataList[5][i] = soLuongBanRa.ToString(); // Cập nhật giá trị số lượng bán ra
                                                    Console.WriteLine(loiNhuan);
                                                    Console.WriteLine($"Cập nhật giá trị Số lượng bán ra và Lợi nhuận thành công!");
                                                    SaveToCsv();
                                                }
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    Console.Write($"Nhập giá trị mới cho {columnDataList[columnIndex - 1][0]} của hàng {columnDataList[0][rowIndex]}: ");
                                    string newValue = Console.ReadLine();
                                    
                                    if (columnDataList[columnIndex - 1].Count >= rowIndex)
                                    {
                                        columnDataList[columnIndex - 1][rowIndex] = newValue;
                                        Console.WriteLine($"Giá trị đã được chỉnh sửa!");
                                        SaveToCsv();
                                    }
                                    else
                                    {
                                        Yellow();
                                        Console.WriteLine("Số hàng không hợp lệ cho cột này. Nhấn Enter để tiếp tục.");
                                        Console.ReadKey();
                                    }
                                }
                            }
                            else
                            {
                                Yellow();
                                Console.WriteLine("Số cột không hợp lệ. Nhấn Enter để tiếp tục.");
                                Console.ReadKey();
                            }
                        }
                    }
                    else
                    {
                        Yellow();
                        Console.WriteLine("Lựa chọn không hợp lệ. Nhấn Enter để tiếp tục.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Yellow();
                    Console.WriteLine("Số hàng không hợp lệ. Nhấn Enter để tiếp tục.");
                    Console.ReadKey();
                }
            }
            else
            {
                Yellow();
                Console.WriteLine("Không tìm thấy mã sản phẩm. Nhấn Enter để tiếp tục.");
                Console.ReadKey();
            }
        }
    }


    // Hàm xóa một hàng khỏi danh sách cột

    static void DeleteRow()
    {
        List<string> maspList = new List<string>();
        string masp;
        for (int columnNumber = 0; columnNumber < columnDataList[0].Count; columnNumber++)
        {
            masp = columnDataList[0][columnNumber];
            maspList.Add(masp);
        }


        Yellow();
        Console.Write("Nhập mã hàng mà bạn muốn xóa: ");
        White();
        string checkCodecheck = Console.ReadLine();
        string checkCode = checkCodecheck.Replace(",", "-");
        if (maspList.Contains(checkCode))
        {
            int rowIndex = maspList.IndexOf(checkCode);

            if (rowIndex != 0)
            {

                White();
                Console.Write($" Bạn có chắc muốn xóa mã hàng ");
                DCyan();
                Console.Write($"{checkCode}");
                White();
                Console.Write(" không? \n" +
                                  "1 - Xóa \n" +
                                  "0 - Thoát \n");

                White();
                string choice1 = Console.ReadLine();
                if (choice1 == "1")
                {
                    foreach (var arr in columnDataList)
                    {
                        if (arr is List<string> list && rowIndex <= list.Count)
                        {
                            list.RemoveAt(rowIndex);
                        }
                    }


                    White();
                    Console.WriteLine($"Mã hàng ");
                    DCyan();
                    Console.Write($"{checkCode}");
                    White();
                    Console.WriteLine(" đã được xóa! Nhấn [Enter] để tiếp tục.");
                    SaveToCsv();
                    Console.ReadKey();
                }
                else if (choice1 == "0")
                {
                    Yellow();
                    Console.WriteLine(" Đã đóng chức năng!");
                    return;
                }
                else
                {
                    Yellow();
                    Console.WriteLine("Lựa chọn không phù hợp, vui lòng chọn lại [Enter].");
                    Console.ReadKey();
                }
            }
        }
        else
        {
            Yellow();
            Console.WriteLine("Số hàng không hợp lệ hoặc vượt quá số hàng hiện có.");
            Console.ReadKey();
        }
    }
    
    static void StatsticticSumarize()
    {
        double doanhthumax = double.MinValue;
        double doanhthumin = double.MaxValue;
        List<string> dsdoanhthumax = new List<string>();
        List<string> dsdoanhthumin = new List<string>();
        int spdabanNhieuNhat = int.MinValue;
        int spdabanItNhat = int.MaxValue;
        List<string> dsdoanhthuMax = new List<string>();
        List<string> dsdoanhthuMin = new List<string>();

        // Lặp qua danh sách sản phẩm
        for (int i = 1; i < GetMaxLength(); i++)
        {
            string sanpham = columnDataList[1][i].Replace("\"", ""); // Lấy tên sản phẩm từ cột "Tên sản phẩm"
            if (double.TryParse(columnDataList[6][i], out double doanhthu)) // Lấy doanh thu từ cột "Doanh thu"
            {
                if (doanhthu > doanhthumax)
                {
                    doanhthumax = doanhthu;
                    dsdoanhthumax.Clear(); // Xóa danh sách hiện tại
                    dsdoanhthumax.Add(sanpham); // Thêm sản phẩm mới có doanh thu cao nhất
                }
                else if (doanhthu == doanhthumax)
                {
                    dsdoanhthumax.Add(sanpham); // Thêm sản phẩm có doanh thu cao nhất vào danh sách
                }

                if (doanhthu < doanhthumin)
                {
                    doanhthumin = doanhthu;
                    dsdoanhthumin.Clear(); // Xóa danh sách hiện tại
                    dsdoanhthumin.Add(sanpham); // Thêm sản phẩm mới có doanh thu thấp nhất
                }
                else if (doanhthu == doanhthumin)
                {
                    dsdoanhthumin.Add(sanpham); // Thêm sản phẩm có doanh thu thấp nhất vào danh sách
                }
            }

            if (int.TryParse(columnDataList[5][i], out int spdaban)) // Lấy số lượng bán ra từ cột "Số lượng bán ra"
            {
                if (spdaban > spdabanNhieuNhat)
                {
                    spdabanNhieuNhat = spdaban;
                    dsdoanhthuMax.Clear(); // Xóa danh sách hiện tại
                    dsdoanhthuMax.Add(sanpham); // Thêm sản phẩm mới có số lượng bán nhiều nhất
                }
                else if (spdaban == spdabanNhieuNhat)
                {
                    dsdoanhthuMax.Add(sanpham); // Thêm sản phẩm có số lượng bán nhiều nhất vào danh sách
                }

                if (spdaban < spdabanItNhat)
                {
                    spdabanItNhat = spdaban;
                    dsdoanhthuMin.Clear(); // Xóa danh sách hiện tại
                    dsdoanhthuMin.Add(sanpham); // Thêm sản phẩm mới có số lượng bán thấp nhất
                }
                else if (spdaban == spdabanItNhat)
                {
                    dsdoanhthuMin.Add(sanpham); // Thêm sản phẩm có số lượng bán thấp nhất vào danh sách
                }
            }
        }
        Yellow();
        Console.Write("Sản phẩm có doanh thu cao nhất là: ");
        DCyan();
        int count = 0;
        foreach (var sanpham in dsdoanhthumax)
        {
            if (count==dsdoanhthumax.Count-1)
            {
                Console.Write(sanpham + " ");
            }
            else
            {
                Console.Write(sanpham + ",");
                count++;
            }
            
        }

        White();
        Console.Write("với doanh thu ");
        DCyan();
        Console.WriteLine(doanhthumax);

        Yellow();
        Console.Write("Sản phẩm có doanh thu thấp nhất là: ");
        DCyan();
        count = 0;
        foreach (var sanpham in dsdoanhthumin)
        {
            if (count==dsdoanhthumin.Count-1)
            {
                Console.Write(sanpham + " ");
            }
            else
            {
                Console.Write(sanpham + ",");
                count++;
            }
            
        }
        White();
        Console.Write("với doanh thu ");
        DCyan();
        Console.WriteLine(doanhthumin);
        Yellow();
        Console.Write("Sản phẩm bán nhiều nhất là: ");
        DCyan();
        count = 0;
        foreach (var sanpham in dsdoanhthuMax)
        {
            if (count==dsdoanhthuMax.Count-1)
            {
                Console.Write(sanpham + " ");
            }
            else
            {
                Console.Write(sanpham + ",");
                count++;
            }
           
        }
        White();
        Console.Write("với ");
        DCyan();
        Console.Write(spdabanNhieuNhat+" sản phẩm");
        White();
        Console.WriteLine(" đã bán");

        Yellow();
        Console.Write("Sản phẩm bán thấp nhất là: ");
        DCyan();
        count = 0;
        foreach (var sanpham in dsdoanhthuMin)
        {
            if (count==dsdoanhthuMin.Count-1)
            {
                Console.Write(sanpham + " ");
            }
            else
            {
                Console.Write(sanpham + ",");
                count++;
            }
            
        }
        White();
        Console.Write("với ");
        DCyan();
        Console.Write(spdabanItNhat+" sản phẩm");
        White();
        Console.WriteLine(" đã bán");
    }
    // Hàm lưu dữ liệu vào file CSV
    static void SaveToCsv()
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(csvFilePath))
            {
                for (int i = 0; i < GetMaxLength(); i++)
                {
                    for (int j = 0; j < columnDataList.Count; j++)
                    {
                        if (columnDataList[j].Count > i)
                        {
                            string value = columnDataList[j][i];
                            writer.Write($"{value}");
                            if (j < columnDataList.Count - 1)
                            {
                                writer.Write(",");
                            }
                        }
                    }

                    writer.WriteLine();
                }
            }

            Yellow();
            Console.WriteLine("Dữ liệu đã được lưu vào file quản lý.");
        }
        catch (IOException e)
        {
            Console.WriteLine("Lỗi khi ghi vào file: " + e.Message);
        }
    }

    // Hàm chính
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
      
        if (File.Exists(csvFilePath))
        {
            FileComposer();
            Console.WriteLine("MENU :");
            // int a = 0;
            while (true)
            {
                Yellow();
                Console.Clear();
                TableDisplay();
                DCyan();
                Console.WriteLine("\n------ CHƯƠNG TRÌNH QUẢN LÝ SẢN PHẨM ------\n");
                White();
                Console.WriteLine("1. Tìm kiếm sản phẩm");
                Console.WriteLine("2. Thêm sản phẩm");
                Console.WriteLine("3. Sửa thông tin sản phẩm");
                Console.WriteLine("4. Xoá sản phẩm");
                Console.WriteLine("5. Thống kê tình hình kinh doanh");
                Console.WriteLine("6. Thoát");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("Chọn chức năng (1-5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        FindInformation();
                        break;
                    case "2":
                        AddRow();
                        Console.Clear();
                        break;
                    case "3":

                        EditCellValue();
                        Console.Clear();
                        break;
                    case "4":

                        DeleteRow();
                        Console.Clear();
                        break;
                    case "5":
                        DrawProfitHistogram();
                        Console.WriteLine("");
                        DrawSalesHistogram();
                        Console.WriteLine("");
                        StatsticticSumarize();
                        break;
                    case "6":
                        Console.WriteLine("Chương trình kết thúc.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Chức năng không hợp lệ. Vui lòng chọn lại.");
                        break;
                }

                Console.WriteLine("Nhấn phím bất kỳ để tiếp tục...");
                Console.ReadKey();
            }
        }
    }
}
