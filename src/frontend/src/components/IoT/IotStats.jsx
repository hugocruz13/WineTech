import styles from "../../styles/IotClientePage.module.css";

const max = (arr, key) =>
  arr.length ? Math.max(...arr.map((i) => i[key])).toFixed(1) : "--";

const min = (arr, key) =>
  arr.length ? Math.min(...arr.map((i) => i[key])).toFixed(1) : "--";

const IotStats = ({ data, dataKey, unit }) => (
  <div className={styles.stats}>
    <div>
      <span>H</span>
      <strong>
        {max(data, dataKey)}
        {unit}
      </strong>
    </div>
    <div>
      <span>L</span>
      <strong>
        {min(data, dataKey)}
        {unit}
      </strong>
    </div>
  </div>
);

export default IotStats;
