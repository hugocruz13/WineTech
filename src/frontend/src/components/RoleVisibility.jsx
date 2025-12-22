import { useAuth0 } from "@auth0/auth0-react";

const RoleVisibility = ({ role, children }) => {
  const { user } = useAuth0();
  const roles = user?.["https://isi.com/roles"] || [];

  return roles.includes(role) ? children : null;
};

export default RoleVisibility;
