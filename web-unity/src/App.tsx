import { useDojo } from './dojo/useDojo';
import './App.css';
import UnityLoader from './ui/components/UnityLoader';
import Connect from './ui/components/Connect';

function App() {
  const {
    account: { account },
  } = useDojo();

  return (
    <div className="flex flex-col gap-4 w-full items-center mt-20">
      {/*<Connect />*/}

      <UnityLoader />
    </div>
  );
}

export default App;
