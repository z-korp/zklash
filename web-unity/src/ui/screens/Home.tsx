import UnityLoader from "../containers/UnityLoader";

const Home = () => {
  return (
    <div className="relative flex flex-col w-screen">
      <div className="relative flex flex-col gap-8 grow items-center justify-start">
        <div className="absolute top-0 flex flex-col items-center gap-4 w-full px-4 py-2 max-w-4xl mx-auto mt-10">
          <UnityLoader />
        </div>
      </div>
    </div>
  );
};

export default Home;
