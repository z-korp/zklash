import * as React from "react";
import { Slot } from "@radix-ui/react-slot";
import { cva, type VariantProps } from "class-variance-authority";
import { cn } from "@/ui/utils";

const buttonVariants = cva(
  "inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium transition-colors focus-visible:outline-none focus-visible:ring-1 focus-visible:ring-ring disabled:pointer-events-none disabled:opacity-50",
  {
    variants: {
      variant: {
        default: "bg-primary text-primary-foreground hover:bg-primary/90",
        destructive:
          "bg-destructive text-destructive-foreground hover:bg-destructive/90",
        outline:
          "border border-input bg-background hover:bg-accent hover:text-accent-foreground",
        secondary:
          "bg-secondary text-secondary-foreground hover:bg-secondary/80",
        ghost: "hover:bg-accent hover:text-accent-foreground",
        link: "text-primary underline-offset-4 hover:underline",
        blue: "bg-blue-500 text-white",
        green: "bg-green-500 text-white",
        yellow: "bg-yellow-500 text-white",
        red: "bg-red-500 text-white",
        white: "bg-white text-black",
      },
      size: {
        default: "h-9 px-4 py-2",
        sm: "h-8 rounded-md px-3 text-xs",
        lg: "h-10 rounded-md px-8",
        icon: "min-h-9 min-w-9",
      },
    },
    defaultVariants: {
      variant: "default",
      size: "default",
    },
  },
);

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof buttonVariants> {
  asChild?: boolean;
}

const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant, size, asChild = false, ...props }, ref) => {
    const Comp = asChild ? Slot : "button";

    // Définir la couleur de l'ombre en fonction de la variante
    const shadowColor =
      {
        white: "rgba(255, 255, 255, 0.6)",
        blue: "rgba(0, 0, 255, 0.6)",
        green: "rgba(0, 255, 0, 0.6)",
        yellow: "rgba(255, 255, 0, 0.6)",
        red: "rgba(255, 0, 0, 0.6)",
      }[variant as keyof typeof buttonVariants.variants.variant] ||
      ("rgba(0, 0, 0, 0.1)" as string);

    return (
      <div
        className={`border-2 border-black p-[2px] rounded-md bg-white hover:translate-y-0.5 transition-transform`}
        style={{ boxShadow: `0 2px 0px ${shadowColor}, 0 4px 0px black` }}
      >
        <Comp
          className={cn(
            buttonVariants({ variant, size, className }),
            "border-2 border-black rounded-md px-2 py-1",
          )}
          ref={ref}
          {...props}
          style={{
            backgroundImage: `url("https://www.transparenttextures.com/patterns/natural-paper.png")`,
          }}
        >
          {props.children}
        </Comp>
      </div>
    );
  },
);
Button.displayName = "Button";

export { Button, buttonVariants };
