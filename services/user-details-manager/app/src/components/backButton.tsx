"use client";
import Link from "next/link";

export default function BackButton({ href = "/" }: { href?: string }) {
  return (
    <Link
      href={href}
      className="flex items-center w-fit mb-4 text-sm font-semibold leading-6 text-zinc-600 hover:text-zinc-900 dark:text-zinc-400 dark:hover:text-zinc-50 transition-colors"
    >
      <span aria-hidden="true" className="mr-2">
        &larr;
      </span>
      Back
    </Link>
  );
}
