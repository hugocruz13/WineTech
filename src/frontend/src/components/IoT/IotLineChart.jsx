import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
  Area,
} from "recharts";
import styles from "../../styles/IotClientePage.module.css";

const IotLineChart = ({ data, dataKey, unit, color }) => {
  if (!data || data.length === 0) return null;

  return (
    <div className={styles.chartWrapper}>
      <ResponsiveContainer width="100%" height="100%">
        <LineChart data={data}>
          <CartesianGrid
            stroke="#e5e7eb"
            strokeDasharray="2 8"
            vertical={false}
          />
          <XAxis
            dataKey="dataHora"
            interval="preserveStartEnd"
            tick={{ fontSize: 11, fill: "#94a3b8" }}
            axisLine={false}
            tickLine={false}
          />

          <YAxis
            domain={["auto", "auto"]}
            tick={{ fontSize: 11, fill: "#94a3b8" }}
            axisLine={false}
            tickLine={false}
          />

          <Tooltip
            cursor={{ stroke: color, strokeOpacity: 0.25 }}
            formatter={(value) => `${value.toFixed(1)} ${unit}`}
            labelStyle={{
              fontSize: 11,
              color: "#64748b",
              marginBottom: 4,
            }}
            contentStyle={{
              backgroundColor: "#ffffff",
              borderRadius: 10,
              border: "1px solid #e5e7eb",
              boxShadow: "0 10px 25px rgba(0,0,0,0.08)",
              fontSize: 12,
              padding: "8px 10px",
            }}
            filterNull={false}
            itemSorter={() => -1}
            isAnimationActive={false}
            content={({ payload, label }) => {
              if (!payload || !payload.length) return null;

              const item = payload[0];

              return (
                <div
                  style={{
                    background: "#fff",
                    borderRadius: 10,
                    border: "1px solid #e5e7eb",
                    boxShadow: "0 10px 25px rgba(0,0,0,0.08)",
                    padding: "8px 10px",
                    fontSize: 12,
                  }}
                >
                  <div
                    style={{ fontSize: 11, color: "#64748b", marginBottom: 4 }}
                  >
                    {label}
                  </div>
                  <div style={{ color: item.color, fontWeight: 500 }}>
                    {item.name}: {item.value.toFixed(1)} {unit}
                  </div>
                </div>
              );
            }}
          />

          <Area
            type="monotone"
            dataKey={dataKey}
            baseValue="dataMin"
            stroke="none"
            fill={color}
            fillOpacity={0.12}
          />

          <Line
            type="monotone"
            dataKey={dataKey}
            stroke={color}
            strokeWidth={2.8}
            dot={false}
            activeDot={{ r: 5 }}
            isAnimationActive
          />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
};

export default IotLineChart;
