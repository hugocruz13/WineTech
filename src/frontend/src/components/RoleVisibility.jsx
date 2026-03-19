import { useAuth0 } from "@auth0/auth0-react";

const RoleVisibility = ({ role, children }) => {
  const { user } = useAuth0();
  const roles = user?.["https://winetech.pt/roles"] || [];

  return roles.includes(role) ? children : null;
};

export default RoleVisibility;
