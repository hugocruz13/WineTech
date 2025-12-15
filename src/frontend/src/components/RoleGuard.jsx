import { useAuth0 } from "@auth0/auth0-react";

const RoleGuard = ({ role, children }) => {
  const { user } = useAuth0();
  const roles = user?.["https://isi.com/roles"] || [];

  if (!roles.includes(role)) {
    return <p>Acesso negado</p>;
  }

  return children;
};

export default RoleGuard;
