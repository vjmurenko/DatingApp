export interface Group {
  name: string;
  connections: Connection[];

}

interface Connection {
  ConnectionId: string;
  userName: string;
}