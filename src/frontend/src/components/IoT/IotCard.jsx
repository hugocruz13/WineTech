import styles from "../../styles/IotClientePage.module.css";

const IotCard = ({ title, icon, children }) => (
  <div className={styles.card}>
    <div className={styles.cardHeader}>
      {icon}
      <strong>{title}</strong>
    </div>
    {children}
  </div>
);

export default IotCard;
