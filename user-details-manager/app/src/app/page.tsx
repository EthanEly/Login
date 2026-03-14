export default function Home() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black">
      <main className="flex min-h-screen w-full max-w-3xl flex-col items-center justify-between py-32 px-16 bg-white dark:bg-black sm:items-start">
        <div className="flex flex-col items-center gap-6 text-center sm:items-start sm:text-left">
          <h1 className="max-w-xs text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Welcome!
          </h1>
          <div className="max-w-md text-2xl text-zinc-600 dark:text-zinc-400">
            <p>Have we met before?</p>
            <p className="max-w-md text-lg py-8">
              If we have, you may{" "}
              <a href="/login" className="font-medium text-zinc-950 dark:text-zinc-50">
                login
              </a>{" "}
              or, if this is your first time, we&apos;ll need you to{" "}
              <a href="/register" className="font-medium text-zinc-950 dark:text-zinc-50">
                register
              </a>
              .
            </p>
          </div>
          <div className="flex flex-col text-base font-medium sm:flex-row">
            <a
              href="/register"
              className="flex h-12 w-full items-center justify-center gap-2 rounded-full bg-foreground px-5 text-background transition-colors hover:bg-[#383838] dark:hover:bg-[#ccc] md:w-[158px]"
            >
              Register
            </a>
            <a
              href="/login"
              className="flex h-12 w-full items-center justify-center rounded-full border border-solid border-black/[.08] px-5 transition-colors hover:border-transparent hover:bg-black/[.04] dark:border-white/[.145] dark:hover:bg-[#1a1a1a] md:w-[158px]"
            >
              Login
            </a>
          </div>
        </div>
      </main>
    </div>
  );
}
