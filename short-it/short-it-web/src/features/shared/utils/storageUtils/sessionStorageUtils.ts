export const getSessionItem = <T = unknown>(
  key: string,
  isJson: boolean = false
): T | string | null => {
  try {
    const item = sessionStorage.getItem(key);

    if (null === item) {
      return null;
    }

    return isJson ? (JSON.parse(item) as T) : item;
  } catch (error) {
    console.error(
      `Failed to get item with key: ${key} from the session storage.`
    );

    return null;
  }
};

export const setSessionItem = (
  key: string,
  value: any,
  isJson: boolean = false
): void => {
  try {
    const data = isJson ? JSON.stringify(value) : value;

    sessionStorage.setItem(key, data);
  } catch (error) {
    console.error(
      `Failed to set item with key: ${key} to the session storage.`
    );
  }
};
