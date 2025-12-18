import styles from "../styles/Loading.module.css";

const Loading = ({ size = 32 }) => {
  return (
    <div className={styles.loadingWrapper} aria-label="A carregar">
      <span
        className={styles.loadingSpinner}
        style={{ width: size, height: size }}
      />
    </div>
  );
};

export default Loading;
