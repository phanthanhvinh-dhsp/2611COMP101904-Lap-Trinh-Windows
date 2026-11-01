using System.Data;
using Microsoft.Data.SqlClient;

public class KetQuaDAL
{
    DataTable Query(string proc, params (string name, object value)[] ps)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        using var cmd = new SqlCommand(proc, conn) { CommandType = CommandType.StoredProcedure };
        foreach (var (name, value) in ps) cmd.Parameters.AddWithValue(name, value);
        using var da = new SqlDataAdapter(cmd);
        var table = new DataTable();
        da.Fill(table);
        return table;
    }

    public DataTable GetLop() => Query("sp_Lop_GetAll");

    public DataTable GetMonHoc() => Query("sp_MonHoc_GetAll");

    public DataTable GetKetQuaByLopMon(string maLop, string maMon) => Query("sp_KetQua_GetByLopMon", ("@MaLop", maLop), ("@MaMon", maMon));

    public int SaveAll(string maMon, DataTable table)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        conn.Open();
        using var trans = conn.BeginTransaction();
        try
        {
            int count = 0;
            foreach (DataRow row in table.Rows)
            {
                using var cmd = new SqlCommand("sp_KetQua_Save", conn, trans) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@MaSV", row["MaSV"].ToString());
                cmd.Parameters.AddWithValue("@MaMon", maMon);
                cmd.Parameters.AddWithValue("@Diem", Convert.ToDouble(row["Diem"]));
                cmd.Parameters.AddWithValue("@GhiChu", row["GhiChu"].ToString());
                cmd.ExecuteNonQuery();
                count++;
            }
            trans.Commit();
            return count;
        }
        catch
        {
            trans.Rollback();
            throw;
        }
    }
}
