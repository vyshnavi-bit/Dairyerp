using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public class GridViewGroup
{
    private int m_columnIndex;
    private GridViewGroup m_parentGroup;
    private string m_groupingColumn;
    private GridView m_gridView;

    private GridViewRow m_firstGroupGridRow = null;
    private DataRow m_firstGroupDataRow = null;
    private int m_rowSpan = 0;
    private bool m_groupChanged;

    public GridViewGroup(GridView gridView, GridViewGroup parentGroup, string groupingColumn)
    {
        if (parentGroup != null)
        {
            m_columnIndex = parentGroup.ColumnIndex + 1;
        }
        m_parentGroup = parentGroup;
        m_groupingColumn = groupingColumn;
        m_gridView = gridView;
        HookGrid();
    }

    private void HookGrid()
    {
        m_gridView.RowDataBound += new GridViewRowEventHandler(GridView_RowDataBound);
        m_gridView.DataBound += new EventHandler(GridView_DataBound);
    }

    private void GridView_DataBound(object sender, EventArgs e)
    {
        if (m_rowSpan > 1)
        {
            m_firstGroupGridRow.Cells[m_columnIndex].RowSpan = m_rowSpan;
        }
    }

    private void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        m_groupChanged = false;
        DataRowView currentDataRowView = (DataRowView)e.Row.DataItem;
        if (currentDataRowView != null)
        {
            DataRow currentDataRow = currentDataRowView.Row;
            if (m_firstGroupDataRow != null)
            {
                object lastValue = m_firstGroupDataRow[m_groupingColumn];
                object currentValue = currentDataRow[m_groupingColumn];
                if (currentValue.Equals(lastValue) && !(m_parentGroup != null && m_parentGroup.GroupChanged))
                {
                    m_rowSpan++;
                    e.Row.Cells[m_columnIndex].Visible = false;
                    return;
                }
                else
                {
                    if (e.Row.RowIndex > 0 && m_parentGroup == null)
                    {
                        GridViewRow row = m_gridView.Rows[e.Row.RowIndex - 1];
                        row.Style.Add("border-bottom", "1px solid lightgray;");
                    }
                }
                if (m_rowSpan > 1)
                {
                    m_firstGroupGridRow.Cells[m_columnIndex].RowSpan = m_rowSpan;
                    if (e.Row.RowIndex > 0 && m_parentGroup == null)
                    {
                        GridViewRow row = m_gridView.Rows[e.Row.RowIndex - 1];
                        row.Style.Add("border-bottom", "1px solid lightgray;");
                    }
                }
            }
            m_firstGroupGridRow = e.Row;
            m_firstGroupDataRow = currentDataRow;
            m_rowSpan = 1;
            m_groupChanged = true;
        }
    }

    public int ColumnIndex
    {
        get
        {
            return m_columnIndex;
        }
    }

    public bool GroupChanged
    {
        get
        {
            return m_groupChanged;
        }
    }

    public static class GeoCodeCalc
    {
        public const double EarthRadiusInMiles = 3956.0;
        public const double EarthRadiusInKilometers = 6367.0;

        public static double ToRadian(double val) { return val * (Math.PI / 180); }
        public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Kilometers);
        }

        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
        {
            double radius = GeoCodeCalc.EarthRadiusInMiles;

            if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
            return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
        }
    }

    public enum GeoCodeCalcMeasurement : int
    {
        Miles = 0,
        Kilometers = 1
    }
}