import "../styles/Loading.css";

const Loading = ({ size = 32 }) => {
  return (
    <div className="loading-wrapper" aria-label="A carregar">
      <span className="loading-spinner" style={{ width: size, height: size }} />
    </div>
  );
};

export default Loading;
